namespace WarThunderStreamDeckPlugin.Settings;

using System.Text.Json.Serialization;

public sealed class GlobalPluginSettings
{
    [JsonPropertyName("controlsBlkPath")]
    public string? ControlsBlkPath { get; set; }
}
