namespace WarThunderStreamDeckPlugin.Settings;

using System.Text.Json.Serialization;

// Stored per Stream Deck button (per action instance), not per plugin.
// The fallbackKey is the key the plugin presses when the .blk has no keyboard
// binding for this action's primary ID. The user must also bind the same key
// in War Thunder for that action.
public sealed class PerActionSettings
{
    [JsonPropertyName("fallbackKey")]
    public string? FallbackKey { get; set; }
}
