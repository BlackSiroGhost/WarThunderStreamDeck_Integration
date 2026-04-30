namespace WarThunderStreamDeckPlugin.Actions;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SharpDeck;
using SharpDeck.Events.Received;
using WarThunderStreamDeckPlugin.Diagnostics;
using WarThunderStreamDeckPlugin.Input;
using WarThunderStreamDeckPlugin.KeyBindings;
using WarThunderStreamDeckPlugin.Settings;
using WarThunderStreamDeckPlugin.Telemetry;
using FallbackKeys = WarThunderStreamDeckPlugin.Input.FallbackKeys;

// Generic War Thunder action with two background tasks per instance:
//   - poll loop: fetches /state at 4Hz, updates icon state and title.
//   - press worker: drains a semaphore queue, dispatching key taps serially
//     so spam-pressing the deck never blocks the SharpDeck event dispatcher.
public abstract class WarThunderBindingAction : StreamDeckAction
{
    private static readonly TimeSpan PollInterval = TimeSpan.FromMilliseconds(250);

    protected IKeyboardInput Keyboard { get; }
    protected IWarThunderTelemetry Telemetry { get; }

    private SemaphoreSlim _pressSignal = new(0, int.MaxValue);
    private CancellationTokenSource? _workerCts;
    private CancellationTokenSource? _pollCts;
    private int? _lastState;
    private string? _lastTitle;
    private uint? _fallbackVk;

    protected WarThunderBindingAction()
        : this(new SendInputKeyboard(), new WarThunderTelemetry())
    {
    }

    protected WarThunderBindingAction(IKeyboardInput keyboard, IWarThunderTelemetry telemetry)
    {
        Keyboard = keyboard;
        Telemetry = telemetry;
    }

    protected abstract string TitlePrefix { get; }

    protected abstract Task<TelemetryReading?> ProbeAsync();

    protected abstract Task OnPressAsync(BindingMap bindings);

    protected override Task OnKeyDown(ActionEventArgs<KeyPayload> args)
    {
        // Hot path: enqueue and return immediately. Spamming the button just
        // releases more permits; the worker tap loop processes them one by one.
        _pressSignal.Release();
        return Task.CompletedTask;
    }

    protected async Task<bool> TryFireBindingAsync(BindingMap map, params string[] bindingIds)
    {
        foreach (var id in bindingIds)
        {
            var dik = map.FirstScanCode(id);
            if (dik is null) continue;

            await Keyboard.TapScanCodeAsync(dik.Value).ConfigureAwait(false);
            return true;
        }

        if (_fallbackVk is uint vk)
        {
            PluginLog.Info($"  no .blk binding for {string.Join(",", bindingIds)} - using fallback vk=0x{vk:X2}");
            await Keyboard.TapVirtualKeyAsync(vk).ConfigureAwait(false);
            return true;
        }

        PluginLog.Warn($"  no scancode for any of: {string.Join(",", bindingIds)} and no fallback set -> alert");
        await ShowAlertAsync().ConfigureAwait(false);
        return false;
    }

    protected override async Task OnWillAppear(ActionEventArgs<AppearancePayload> args)
    {
        PluginLog.Info($"OnWillAppear action={GetType().Name}");
        UpdateFallbackFromSettings(args.Payload?.GetSettings<PerActionSettings>());
        EnsureSharedConfigWatcher();
        await EnsureGlobalSettingsLoadedAsync().ConfigureAwait(false);
        StartWorker();
        StartPolling();
    }

    protected override async Task OnSendToPlugin(ActionEventArgs<JObject> args)
    {
        try
        {
            var payload = args.Payload ?? new JObject();
            var type = payload["type"]?.ToString();
            switch (type)
            {
                case "browse":
                {
                    var current = SharedConfig.Read().ControlsBlkPath ?? BindingsCache.Instance.CurrentPath;
                    var picked = BlkFilePicker.Pick(current);
                    if (!string.IsNullOrEmpty(picked))
                        await SendToPropertyInspectorAsync(new { type = "browseResult", path = picked }).ConfigureAwait(false);
                    else
                        await SendToPropertyInspectorAsync(new { type = "browseCancelled" }).ConfigureAwait(false);
                    break;
                }
                case "test":
                {
                    var path = payload["path"]?.ToString();
                    var (ok, message) = TestBlkPath(path);
                    await SendToPropertyInspectorAsync(new { type = "testResult", ok, message }).ConfigureAwait(false);
                    break;
                }
                case "setPath":
                {
                    var path = payload["path"]?.ToString() ?? string.Empty;
                    var p = SharedConfig.Read();
                    p.ControlsBlkPath = path;
                    SharedConfig.Write(p);
                    await EnsureGlobalSettingsLoadedAsync().ConfigureAwait(false);
                    await SendToPropertyInspectorAsync(new { type = "pathSaved", path }).ConfigureAwait(false);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            PluginLog.Error("OnSendToPlugin failed", ex);
        }
    }

    private static (bool ok, string message) TestBlkPath(string? path)
    {
        var resolved = ControlsPathResolver.Resolve(path);
        if (string.IsNullOrEmpty(resolved))
            return (false, "Path is empty and auto-detect found no .blk.");
        if (!File.Exists(resolved))
            return (false, $"File not found: {resolved}");
        try
        {
            var map = BlkParser.LoadFromFile(resolved);
            if (map.Ids.Count == 0)
                return (false, $"Parsed file but no bindings recognised. Path: {resolved}");
            return (true, $"OK - {map.Ids.Count} bindings loaded from {resolved}");
        }
        catch (Exception ex)
        {
            return (false, $"Could not parse: {ex.Message}");
        }
    }

    private static int _sharedWatcherStarted;
    private void EnsureSharedConfigWatcher()
    {
        if (Interlocked.Exchange(ref _sharedWatcherStarted, 1) != 0) return;
        SharedConfig.StartWatcher();
        SharedConfig.Changed += () =>
        {
            try { _ = EnsureGlobalSettingsLoadedAsync(); } catch { }
        };
    }

    protected override Task OnWillDisappear(ActionEventArgs<AppearancePayload> args)
    {
        StopPolling();
        StopWorker();
        return base.OnWillDisappear(args);
    }

    protected override Task OnDidReceiveSettings(ActionEventArgs<ActionPayload> args)
    {
        UpdateFallbackFromSettings(args.Payload?.GetSettings<PerActionSettings>());
        return base.OnDidReceiveSettings(args);
    }

    private void UpdateFallbackFromSettings(PerActionSettings? s)
    {
        var prev = _fallbackVk;
        _fallbackVk = FallbackKeys.Lookup(s?.FallbackKey);
        if (prev != _fallbackVk)
            PluginLog.Info($"  {GetType().Name} fallback key set to '{s?.FallbackKey ?? "<none>"}' (vk={(_fallbackVk?.ToString("X2") ?? "<null>")})");
    }

    private void StartWorker()
    {
        StopWorker();
        _pressSignal = new SemaphoreSlim(0, int.MaxValue);
        var cts = new CancellationTokenSource();
        _workerCts = cts;
        _ = Task.Run(() => WorkerLoopAsync(cts.Token));
    }

    private void StopWorker()
    {
        var cts = Interlocked.Exchange(ref _workerCts, null);
        if (cts is null) return;
        try { cts.Cancel(); cts.Dispose(); } catch { }
    }

    private async Task WorkerLoopAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try { await _pressSignal.WaitAsync(ct).ConfigureAwait(false); }
            catch (OperationCanceledException) { break; }

            try
            {
                var map = BindingsCache.Instance.GetMap();
                if (map.Ids.Count == 0)
                {
                    await EnsureGlobalSettingsLoadedAsync().ConfigureAwait(false);
                    map = BindingsCache.Instance.GetMap();
                }
                await OnPressAsync(map).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                PluginLog.Error($"press worker ({GetType().Name})", ex);
            }
        }
    }

    private void StartPolling()
    {
        StopPolling();
        var cts = new CancellationTokenSource();
        _pollCts = cts;
        _ = Task.Run(() => PollLoopAsync(cts.Token));
    }

    private void StopPolling()
    {
        var cts = Interlocked.Exchange(ref _pollCts, null);
        if (cts is null) return;
        try { cts.Cancel(); cts.Dispose(); } catch { }
    }

    private async Task PollLoopAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try { await PushReadingAsync().ConfigureAwait(false); }
            catch (Exception ex) { PluginLog.Error("poll tick threw", ex); }
            try { await Task.Delay(PollInterval, ct).ConfigureAwait(false); }
            catch (OperationCanceledException) { break; }
        }
    }

    private async Task PushReadingAsync()
    {
        // No controls path configured: this is the friend-just-installed-it case.
        // Surface a clear setup hint on the button itself.
        if (BindingsCache.Instance.CurrentPath is null)
        {
            await SetTitleIfChangedAsync($"SETUP\n{TitlePrefix}").ConfigureAwait(false);
            return;
        }

        TelemetryReading? reading;
        try { reading = await ProbeAsync().ConfigureAwait(false); }
        catch { reading = null; }

        if (reading is null)
        {
            await SetTitleIfChangedAsync($"{TitlePrefix}\n--").ConfigureAwait(false);
            return;
        }

        var r = reading.Value;
        if (_lastState != r.State)
        {
            await SetStateAsync(r.State).ConfigureAwait(false);
            _lastState = r.State;
        }

        var title = r.Percent is null
            ? TitlePrefix
            : $"{TitlePrefix}\n{(int)Math.Round(r.Percent.Value)}%";
        await SetTitleIfChangedAsync(title).ConfigureAwait(false);
    }

    private async Task SetTitleIfChangedAsync(string title)
    {
        if (_lastTitle == title) return;
        await SetTitleAsync(title).ConfigureAwait(false);
        _lastTitle = title;
    }

    private async Task EnsureGlobalSettingsLoadedAsync()
    {
        try
        {
            // Priority: SharedConfig (cross-plugin) -> per-plugin global -> auto-detect.
            var shared = SharedConfig.Read().ControlsBlkPath;
            string? raw = shared;
            if (string.IsNullOrWhiteSpace(raw) && StreamDeck is not null)
            {
                var settings = await StreamDeck.GetGlobalSettingsAsync<GlobalPluginSettings>().ConfigureAwait(false);
                raw = settings?.ControlsBlkPath;
            }

            var resolved = ControlsPathResolver.Resolve(raw);
            var prev = BindingsCache.Instance.CurrentPath;
            BindingsCache.Instance.SetPath(resolved);
            if (!string.Equals(prev, resolved, StringComparison.OrdinalIgnoreCase))
            {
                PluginLog.Info($"controls path resolved -> {resolved ?? "<none>"} (raw='{raw ?? "<null>"}', source={(string.IsNullOrWhiteSpace(shared) ? "auto/per-plugin" : "shared")})");
            }
        }
        catch (Exception ex)
        {
            PluginLog.Error("EnsureGlobalSettingsLoadedAsync threw", ex);
        }
    }
}
