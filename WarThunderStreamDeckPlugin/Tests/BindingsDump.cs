using System.Reflection;
using System.Text.RegularExpressions;
using WarThunderStreamDeckPlugin.KeyBindings;
using WarThunderStreamDeckPlugin.Services;

namespace WarThunderStreamDeckPlugin.Tests;

// Simple helper to print property -> mapped key(s) from the current .blk
public static class BindingsDump
{
    public static void Print()
    {
        string path;
        try { path = ControlsPathStore.Instance.RequirePath(); }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }

        var kb = KeyBindingsFiller.LoadFromBlk(path);
        string text;
        try { text = File.ReadAllText(path); } catch { text = string.Empty; }
        var hotkeys = ExtractSection(text, "hotkeys") ?? string.Empty;

        var type = typeof(WarThunderStreamDeckPlugin.KeyBindings.KeyBindings);
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var p in props)
        {
            var value = p.GetValue(kb) as string;
            var keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrWhiteSpace(value)) keys.Add(value!);
            foreach (var k in GetKeyboardKeysForId(hotkeys, p.Name)) keys.Add(k);

            if (keys.Count > 0)
            {
                Console.WriteLine($"{p.Name}: {string.Join(", ", keys)}");
            }
        }
    }

    private static IEnumerable<string> GetKeyboardKeysForId(string hotkeys, string id)
    {
        var block = FindBlock(hotkeys, id);
        if (block is null) yield break;
        var rx = new Regex(@"keyboardKey\\s*:\\s*i=(\\d+)", RegexOptions.IgnoreCase);
        foreach (Match m in rx.Matches(block))
        {
            if (m.Success) yield return m.Groups[1].Value;
        }
    }

    private static string? FindBlock(string text, string name)
    {
        var idx = text.IndexOf(name + "{", StringComparison.OrdinalIgnoreCase);
        if (idx < 0) return null;
        int start = text.IndexOf('{', idx);
        if (start < 0) return null;
        int depth = 0;
        for (int i = start; i < text.Length; i++)
        {
            if (text[i] == '{') depth++;
            else if (text[i] == '}')
            {
                depth--;
                if (depth == 0)
                    return text.Substring(start + 1, i - start - 1);
            }
        }
        return null;
    }

    private static string? ExtractSection(string text, string sectionName)
    {
        var idx = text.IndexOf(sectionName + "{", StringComparison.OrdinalIgnoreCase);
        if (idx < 0) return null;
        int start = text.IndexOf('{', idx);
        if (start < 0) return null;
        int depth = 0;
        for (int i = start; i < text.Length; i++)
        {
            if (text[i] == '{') depth++;
            else if (text[i] == '}')
            {
                depth--;
                if (depth == 0)
                    return text.Substring(start + 1, i - start - 1);
            }
        }
        return null;
    }
}
