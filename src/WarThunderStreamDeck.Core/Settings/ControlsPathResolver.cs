namespace WarThunderStreamDeckPlugin.Settings;

using System;
using System.IO;

// Picks the .blk to use. Priority:
//   1. User-supplied path from global settings (if file exists).
//   2. The standard War Thunder local controls path under the user's Documents folder.
//   3. Any .blk shipped next to the plugin executable (dev fallback).
public static class ControlsPathResolver
{
    public static string? Resolve(string? userPath)
    {
        var fromSettings = Normalize(userPath);
        if (!string.IsNullOrWhiteSpace(fromSettings) && File.Exists(fromSettings))
        {
            Diagnostics.PluginLog.Info($"resolver: using user-set path: {fromSettings}");
            return fromSettings;
        }

        foreach (var candidate in DefaultCandidates())
        {
            if (File.Exists(candidate))
            {
                Diagnostics.PluginLog.Info($"resolver: auto-discovered {candidate}");
                return candidate;
            }
        }
        Diagnostics.PluginLog.Warn("resolver: no .blk found via auto-discovery");
        return null;
    }

    private static string? Normalize(string? path)
    {
        if (string.IsNullOrWhiteSpace(path)) return null;
        var p = path.Trim().Trim('"');
        try
        {
            p = Environment.ExpandEnvironmentVariables(p);
            if (!Path.IsPathRooted(p)) p = Path.GetFullPath(p);
        }
        catch { }
        return p;
    }

    private static IEnumerable<string> DefaultCandidates()
    {
        var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        if (!string.IsNullOrEmpty(docs))
        {
            // Modern WT layout: per-account snapshot, with "last" symlinking the active one.
            var savesRoot = Path.Combine(docs, "My Games", "WarThunder", "Saves");
            yield return Path.Combine(savesRoot, "last", "production", "machine.blk");
            foreach (var hit in SafeEnumerate(savesRoot, "*", SearchOption.TopDirectoryOnly, dirsOnly: true))
            {
                var perAccount = Path.Combine(hit, "production", "machine.blk");
                if (File.Exists(perAccount)) yield return perAccount;
            }

            // Older / alternate layout
            yield return Path.Combine(docs, "My Games", "WarThunder", "Config", "controls.blk");
            yield return Path.Combine(docs, "My Games", "WarThunder", "Config", "wtcontrols.blk");
        }

        var exe = Path.GetDirectoryName(Environment.ProcessPath);
        if (!string.IsNullOrEmpty(exe))
        {
            foreach (var hit in SafeEnumerate(exe, "*.blk", SearchOption.TopDirectoryOnly, dirsOnly: false))
                yield return hit;
        }
    }

    private static IEnumerable<string> SafeEnumerate(string dir, string pattern, SearchOption opt, bool dirsOnly)
    {
        try
        {
            return dirsOnly
                ? Directory.EnumerateDirectories(dir, pattern, opt)
                : Directory.EnumerateFiles(dir, pattern, opt);
        }
        catch { return Array.Empty<string>(); }
    }
}
