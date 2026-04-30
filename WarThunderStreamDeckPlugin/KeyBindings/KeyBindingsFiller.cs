using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WarThunderStreamDeckPlugin.KeyBindings
{
    // Reads a .blk file and fills KeyBindings properties by matching property-name to action id found in the file
    public static class KeyBindingsFiller
    {
        public static KeyBindings LoadFromBlk(string path)
        {
            var kb = new KeyBindings();
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return kb;

            string text;
            try { text = File.ReadAllText(path); }
            catch { return kb; }

            // parse only the hotkeys section to map id->keyboardKey index
            var hotkeys = ExtractSection(text, "hotkeys");
            if (hotkeys is null) return kb;

            var idBlocks = SplitBlocks(hotkeys);

            var props = typeof(KeyBindings)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

            foreach (var block in idBlocks)
            {
                var id = ExtractBlockName(block);
                if (string.IsNullOrWhiteSpace(id)) continue;

                // Prefer a keyboardKey if present; ignore mouse/joy for StreamDeck
                var keyToken = TryGetValue(block, "keyboardKey")
                            ?? TryGetValue(block, "key")
                            ?? TryGetValue(block, "keyid")
                            ?? TryGetValue(block, "vkey")
                            ?? TryGetValue(block, "button");

                if (string.IsNullOrWhiteSpace(keyToken)) continue;

                // The sample uses keyboardKey:i=<scanCode>; we keep the numeric token as string
                if (props.TryGetValue(id!, out var pi) && pi.CanWrite)
                {
                    pi.SetValue(kb, keyToken);
                }
            }

            return kb;
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

        private static IEnumerable<string> SplitBlocks(string text)
        {
            // Splits top-level blocks like ID_XXX{ ... }
            var blocks = new List<string>();
            int i = 0;
            while (i < text.Length)
            {
                // find next name{
                int nameStart = i;
                while (nameStart < text.Length && char.IsWhiteSpace(text[nameStart])) nameStart++;
                if (nameStart >= text.Length) break;

                int brace = text.IndexOf('{', nameStart);
                if (brace < 0) break;
                int depth = 1;
                int end = brace + 1;
                while (end < text.Length && depth > 0)
                {
                    if (text[end] == '{') depth++;
                    else if (text[end] == '}') depth--;
                    end++;
                }
                if (depth == 0)
                {
                    blocks.Add(text.Substring(nameStart, end - nameStart));
                    i = end;
                }
                else break;
            }
            return blocks;
        }

        private static string? ExtractBlockName(string block)
        {
            int i = 0;
            while (i < block.Length && char.IsWhiteSpace(block[i])) i++;
            int end = block.IndexOf('{', i);
            if (end < 0) return null;
            return block.Substring(i, end - i).Trim();
        }

        private static string? TryGetValue(string block, string key)
        {
            // Example lines inside block:
            // keyboardKey:i=10
            // keyboardKey:i=42
            var idx = block.IndexOf(key + ":", StringComparison.OrdinalIgnoreCase);
            if (idx < 0) return null;
            var rest = block.Substring(idx + key.Length + 1).Trim();
            if (rest.Length == 0) return null;

            // Expect type prefix like i=, t=, r=
            if (rest.Length >= 2 && rest[1] == '=') rest = rest.Substring(2).Trim();

            // capture number or token until whitespace/newline/closing brace
            int cut = rest.IndexOfAny(new[] { ' ', '\t', '\r', '\n', '}' });
            var value = cut > 0 ? rest.Substring(0, cut) : rest;
            return value.Trim('"');
        }
    }
}
