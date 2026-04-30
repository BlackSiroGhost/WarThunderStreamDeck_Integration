namespace WarThunderStreamDeckPlugin.Settings;

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using WarThunderStreamDeckPlugin.Diagnostics;

// Cross-plugin path sharing. Stream Deck's per-plugin global settings are isolated
// per plugin UUID, so each module would normally have to be configured independently.
// To avoid that, every plugin reads/writes the same %LocalAppData%\WarThunderStreamDeck\config.json
// and watches it for changes from sibling plugins.
public static class SharedConfig
{
    private static readonly string Dir =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WarThunderStreamDeck");
    private static readonly string FilePath = Path.Combine(Dir, "config.json");

    private static FileSystemWatcher? _watcher;

    public sealed class Payload
    {
        [JsonPropertyName("schemaVersion")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("controlsBlkPath")]
        public string? ControlsBlkPath { get; set; }
    }

    public static event Action? Changed;

    public static Payload Read()
    {
        try
        {
            if (!File.Exists(FilePath)) return new Payload();
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<Payload>(json) ?? new Payload();
        }
        catch (Exception ex)
        {
            PluginLog.Error("SharedConfig.Read failed", ex);
            return new Payload();
        }
    }

    public static void Write(Payload p)
    {
        try
        {
            Directory.CreateDirectory(Dir);
            var json = JsonSerializer.Serialize(p, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
        catch (Exception ex)
        {
            PluginLog.Error("SharedConfig.Write failed", ex);
        }
    }

    public static void StartWatcher()
    {
        if (_watcher is not null) return;
        try
        {
            Directory.CreateDirectory(Dir);
            _watcher = new FileSystemWatcher(Dir, "config.json")
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime,
                EnableRaisingEvents = true
            };
            _watcher.Changed += (_, _) => Changed?.Invoke();
            _watcher.Created += (_, _) => Changed?.Invoke();
            _watcher.Renamed += (_, _) => Changed?.Invoke();
        }
        catch (Exception ex)
        {
            PluginLog.Error("SharedConfig.StartWatcher failed", ex);
        }
    }
}
