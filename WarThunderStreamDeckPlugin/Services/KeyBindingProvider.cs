namespace WarThunderStreamDeckPlugin.Services;

using System.Collections.Concurrent;
using System.Reflection;
using WarThunderStreamDeckPlugin.KeyBindings;

public class BlkKeyBindingProvider : IKeyBindingProvider
{
    private static readonly ConcurrentDictionary<string, (DateTime LastWriteUtc, KeyBindings Bindings)> Cache = new(StringComparer.OrdinalIgnoreCase);

    // Map friendly action names to .blk IDs
    private static readonly Dictionary<string, string[]> ActionAliases = new(StringComparer.OrdinalIgnoreCase)
    {
        { "gear", new[]{ "ID_GEAR" } },
        { "flaps", new[]{ "ID_FLAPS", "ID_FLAPS_DOWN", "ID_FLAPS_UP" } },
        { "air_brake", new[]{ "ID_AIR_BRAKE" } },
    };

    public byte? GetVirtualKeyForAction(string actionName, string? controlsFilePath = null)
    {
        var file = controlsFilePath ?? ControlsPathHelper.ResolveFromSettingsOrEnv(null);
        if (string.IsNullOrWhiteSpace(file) || !File.Exists(file)) return null;

        var kb = GetBindings(file);
        var props = typeof(KeyBindings).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

        // direct id
        if (props.TryGetValue(actionName, out var pi))
        {
            var token = pi.GetValue(kb) as string;
            if (TryMapTokenToVk(token, out var vk)) return vk;
        }

        // aliases
        if (ActionAliases.TryGetValue(actionName, out var list))
        {
            foreach (var id in list)
            {
                if (props.TryGetValue(id, out var prop))
                {
                    var token = prop.GetValue(kb) as string;
                    if (TryMapTokenToVk(token, out var vk)) return vk;
                }
            }
        }

        // fuzzy
        foreach (var p in props.Keys.Where(k => k.Contains(actionName, StringComparison.OrdinalIgnoreCase)))
        {
            var token = props[p].GetValue(kb) as string;
            if (TryMapTokenToVk(token, out var vk)) return vk;
        }

        return null;
    }

    private static KeyBindings GetBindings(string file)
    {
        var fi = new FileInfo(file);
        var last = fi.LastWriteTimeUtc;
        if (Cache.TryGetValue(file, out var c) && c.LastWriteUtc == last) return c.Bindings;
        var loaded = KeyBindingsFiller.LoadFromBlk(file);
        Cache[file] = (last, loaded);
        return loaded;
    }

    private static bool TryMapTokenToVk(string? token, out byte? vk)
    {
        vk = null;
        if (string.IsNullOrWhiteSpace(token)) return false;

        // In your .blk, tokens are numeric keyboardKey scan codes
        if (byte.TryParse(token, out var scan))
        {
            // Map some common scan codes used in the sample to VKs (minimal map)
            var map = new Dictionary<byte, byte>
            {
                // numbers are DIK codes (DirectInput). Map common ones used above
                // 19 (R) 33 (F) 34 (G) 35 (H) etc per DirectInput DIK to VK approximate mapping
                { 2, (byte)'1' },
                { 3, (byte)'2' },
                { 4, (byte)'3' },
                { 5, (byte)'4' },
                { 6, (byte)'5' },
                { 7, (byte)'6' },
                { 8, (byte)'7' },
                { 9, (byte)'8' },
                { 10, (byte)'9' },
                { 11, (byte)'0' },
                { 16, (byte)'Q' },
                { 17, (byte)'W' },
                { 18, (byte)'E' },
                { 19, (byte)'R' },
                { 20, (byte)'T' },
                { 21, (byte)'Y' },
                { 22, (byte)'U' },
                { 23, (byte)'I' },
                { 24, (byte)'O' },
                { 25, (byte)'P' },
                { 30, (byte)'A' },
                { 31, (byte)'S' },
                { 32, (byte)'D' },
                { 33, (byte)'F' },
                { 34, (byte)'G' },
                { 35, (byte)'H' },
                { 36, (byte)'J' },
                { 37, (byte)'K' },
                { 38, (byte)'L' },
                { 44, (byte)'Z' },
                { 45, (byte)'X' },
                { 46, (byte)'C' },
                { 47, (byte)'V' },
                { 48, (byte)'B' },
                { 49, (byte)'N' },
                { 50, (byte)'M' },
                { 57, 0x20 }, // Space
                { 59, 0x70 }, // F1
                { 60, 0x71 }, // F2
                { 61, 0x72 }, // F3
                { 62, 0x73 }, // F4
                { 71, 0x24 }, // Home
                { 72, 0x26 }, // Up
                { 73, 0x21 }, // PageUp
                { 74, 0x25 }, // Left
                { 76, 0x0D }, // Enter (numpad?)
                { 78, 0x27 }, // Right
                { 80, 0x22 }, // PageDown
                { 181, 0x6F }, // Divide (numpad)
                { 183, 0x2C }, // PrintScreen
                { 200, 0x26 }, // Up
                { 203, 0x25 }, // Left
                { 205, 0x27 }, // Right
                { 207, 0x23 }, // End
                { 208, 0x28 }, // Down
            };

            if (map.TryGetValue(scan, out var v)) { vk = v; return true; }
        }

        // Fallback: if token is already a letter/digit or known VK alias
        token = token.Trim('"', '\'', '`');
        if (token.Length == 1)
        {
            char c = char.ToUpperInvariant(token[0]);
            if ((c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9')) { vk = (byte)c; return true; }
        }

        var alias = new Dictionary<string, byte>(StringComparer.OrdinalIgnoreCase)
        {
            { "SPACE", 0x20 }, { "ENTER", 0x0D }, { "RETURN", 0x0D }, { "ESC", 0x1B }, { "TAB", 0x09 },
        };
        if (alias.TryGetValue(token, out var vkA)) { vk = vkA; return true; }

        return false;
    }
}
