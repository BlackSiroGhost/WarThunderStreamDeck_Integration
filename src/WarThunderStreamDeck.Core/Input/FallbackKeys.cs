namespace WarThunderStreamDeckPlugin.Input;

using System;
using System.Collections.Generic;

// Curated list of keys recommended for use as fallbacks when a War Thunder action
// has no keyboard binding in the .blk. F13-F24 don't appear on physical keyboards
// so they collide with nothing else; Pause/ScrollLock are also rarely used.
public static class FallbackKeys
{
    public static readonly IReadOnlyList<string> Names = new[]
    {
        "F13", "F14", "F15", "F16", "F17", "F18",
        "F19", "F20", "F21", "F22", "F23", "F24",
        "ScrollLock", "Pause",
    };

    private static readonly Dictionary<string, uint> NameToVk = new(StringComparer.OrdinalIgnoreCase)
    {
        { "F13", 0x7C }, { "F14", 0x7D }, { "F15", 0x7E }, { "F16", 0x7F },
        { "F17", 0x80 }, { "F18", 0x81 }, { "F19", 0x82 }, { "F20", 0x83 },
        { "F21", 0x84 }, { "F22", 0x85 }, { "F23", 0x86 }, { "F24", 0x87 },
        { "ScrollLock", 0x91 },
        { "Pause", 0x13 },
    };

    public static uint? Lookup(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        return NameToVk.TryGetValue(name.Trim(), out var vk) ? vk : null;
    }
}
