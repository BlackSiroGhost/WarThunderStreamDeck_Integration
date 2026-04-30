namespace WarThunderStreamDeckPlugin.Actions;

using System;
using System.Threading.Tasks;
using SharpDeck;
using SharpDeck.Events.Received;
using WarThunderStreamDeckPlugin.Input;
using WarThunderStreamDeckPlugin.Settings;
using WarThunderStreamDeckPlugin.Telemetry;

// Generic War Thunder action: presses the keyboard key bound to a single .blk ID.
// Subclasses set the ID and (optionally) the telemetry probe used to drive icon state.
public abstract class WarThunderBindingAction : StreamDeckAction
{
    private readonly IKeyboardInput _keyboard;
    private readonly IWarThunderTelemetry _telemetry;
    private bool _subscribed;

    protected WarThunderBindingAction()
        : this(new SendInputKeyboard(), new WarThunderTelemetry())
    {
    }

    protected WarThunderBindingAction(IKeyboardInput keyboard, IWarThunderTelemetry telemetry)
    {
        _keyboard = keyboard;
        _telemetry = telemetry;
    }

    // Primary .blk ID this action presses (e.g. "ID_GEAR").
    protected abstract string PrimaryBindingId { get; }

    // Fallback IDs to try if the primary has no keyboard binding (e.g. "ID_FLAPS_DOWN").
    protected virtual string[] FallbackBindingIds => Array.Empty<string>();

    // Returns the on/off state to display, or null to leave the icon unchanged.
    protected abstract Task<int?> ProbeStateAsync(IWarThunderTelemetry telemetry);

    protected override async Task OnKeyDown(ActionEventArgs<KeyPayload> args)
    {
        await EnsureGlobalSettingsLoadedAsync().ConfigureAwait(false);

        var map = BindingsCache.Instance.GetMap();
        var dik = map.FirstScanCode(PrimaryBindingId);
        if (dik is null)
        {
            foreach (var alt in FallbackBindingIds)
            {
                dik = map.FirstScanCode(alt);
                if (dik is not null) break;
            }
        }

        if (dik is null)
        {
            await ShowAlertAsync().ConfigureAwait(false);
            return;
        }

        _keyboard.TapScanCode(dik.Value);

        var state = await ProbeStateAsync(_telemetry).ConfigureAwait(false);
        if (state is not null) await SetStateAsync(state.Value).ConfigureAwait(false);
    }

    protected override async Task OnWillAppear(ActionEventArgs<AppearancePayload> args)
    {
        SubscribeToGlobalSettings();
        await EnsureGlobalSettingsLoadedAsync().ConfigureAwait(false);

        var state = await ProbeStateAsync(_telemetry).ConfigureAwait(false);
        if (state is not null) await SetStateAsync(state.Value).ConfigureAwait(false);
    }

    protected override Task OnWillDisappear(ActionEventArgs<AppearancePayload> args)
    {
        UnsubscribeFromGlobalSettings();
        return base.OnWillDisappear(args);
    }

    private void SubscribeToGlobalSettings()
    {
        if (_subscribed || StreamDeck is null) return;
        StreamDeck.DidReceiveGlobalSettings += OnGlobalSettingsChanged;
        _subscribed = true;
    }

    private void UnsubscribeFromGlobalSettings()
    {
        if (!_subscribed || StreamDeck is null) return;
        StreamDeck.DidReceiveGlobalSettings -= OnGlobalSettingsChanged;
        _subscribed = false;
    }

    private async void OnGlobalSettingsChanged(object? sender, EventArgs e)
    {
        try { await EnsureGlobalSettingsLoadedAsync().ConfigureAwait(false); }
        catch { /* swallowed; transient */ }
    }

    private async Task EnsureGlobalSettingsLoadedAsync()
    {
        if (StreamDeck is null) return;
        try
        {
            var settings = await StreamDeck.GetGlobalSettingsAsync<GlobalPluginSettings>().ConfigureAwait(false);
            BindingsCache.Instance.SetPath(settings?.ControlsBlkPath);
        }
        catch
        {
            // best effort
        }
    }
}
