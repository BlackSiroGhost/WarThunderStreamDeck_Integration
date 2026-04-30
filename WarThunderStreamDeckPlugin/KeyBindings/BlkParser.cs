namespace WarThunderStreamDeckPlugin.KeyBindings;

using System.Collections.Generic;
using System.IO;

public static class BlkParser
{
    public static BindingMap LoadFromFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            return BindingMap.Empty;

        return Parse(File.ReadAllText(path));
    }

    public static BindingMap Parse(string text)
    {
        var hotkeys = ExtractSection(text, "hotkeys");
        var map = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);
        if (hotkeys is null) return new BindingMap(map);

        foreach (var (id, body) in EnumerateBlocks(hotkeys))
        {
            foreach (var scan in ExtractKeyboardKeys(body))
            {
                if (!map.TryGetValue(id, out var list))
                {
                    list = new List<int>();
                    map[id] = list;
                }
                if (!list.Contains(scan)) list.Add(scan);
            }
        }
        return new BindingMap(map);
    }

    private static string? ExtractSection(string text, string sectionName)
    {
        var idx = text.IndexOf(sectionName + "{", StringComparison.OrdinalIgnoreCase);
        if (idx < 0) return null;
        var start = text.IndexOf('{', idx);
        return start < 0 ? null : ReadBalancedBlock(text, start);
    }

    private static IEnumerable<(string Id, string Body)> EnumerateBlocks(string text)
    {
        var i = 0;
        while (i < text.Length)
        {
            while (i < text.Length && (char.IsWhiteSpace(text[i]) || text[i] == '}')) i++;
            if (i >= text.Length) yield break;

            var brace = text.IndexOf('{', i);
            if (brace < 0) yield break;

            var name = text[i..brace].Trim();
            var body = ReadBalancedBlock(text, brace);
            if (body is null) yield break;

            i = SkipPastBlock(text, brace);
            if (!string.IsNullOrEmpty(name)) yield return (name, body);
        }
    }

    private static string? ReadBalancedBlock(string text, int openBraceIndex)
    {
        var depth = 0;
        for (var i = openBraceIndex; i < text.Length; i++)
        {
            if (text[i] == '{') depth++;
            else if (text[i] == '}')
            {
                depth--;
                if (depth == 0) return text.Substring(openBraceIndex + 1, i - openBraceIndex - 1);
            }
        }
        return null;
    }

    private static int SkipPastBlock(string text, int openBraceIndex)
    {
        var depth = 0;
        for (var i = openBraceIndex; i < text.Length; i++)
        {
            if (text[i] == '{') depth++;
            else if (text[i] == '}')
            {
                depth--;
                if (depth == 0) return i + 1;
            }
        }
        return text.Length;
    }

    private static IEnumerable<int> ExtractKeyboardKeys(string body)
    {
        const string Marker = "keyboardKey:i=";
        var i = 0;
        while (true)
        {
            i = body.IndexOf(Marker, i, StringComparison.OrdinalIgnoreCase);
            if (i < 0) yield break;
            i += Marker.Length;

            var end = i;
            while (end < body.Length && char.IsDigit(body[end])) end++;
            if (end > i && int.TryParse(body[i..end], out var scan))
                yield return scan;
            i = end;
        }
    }
}
