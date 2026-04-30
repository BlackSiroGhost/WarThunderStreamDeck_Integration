namespace WarThunderStreamDeckPlugin.Diagnostics;

using System;
using System.IO;

// Lightweight file logger. Stream Deck swallows stdout, so we need our own trail
// to diagnose anything. Log goes to %LOCALAPPDATA%\WarThunderStreamDeckPlugin\plugin.log.
public static class PluginLog
{
    private static readonly object Gate = new();
    private static readonly string LogDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "WarThunderStreamDeckPlugin");
    private static readonly string LogFile = Path.Combine(LogDir, "plugin.log");

    public static string LogPath => LogFile;

    static PluginLog()
    {
        try { Directory.CreateDirectory(LogDir); } catch { }
        Info($"=== plugin started, pid={Environment.ProcessId}, exe={Environment.ProcessPath} ===");
    }

    public static void Info(string msg) => Write("INFO", msg);
    public static void Warn(string msg) => Write("WARN", msg);
    public static void Error(string msg, Exception? ex = null)
        => Write("ERR ", ex is null ? msg : $"{msg} :: {ex.GetType().Name}: {ex.Message}");

    private static void Write(string level, string msg)
    {
        try
        {
            var line = $"{DateTime.Now:HH:mm:ss.fff} {level} {msg}{Environment.NewLine}";
            lock (Gate)
            {
                File.AppendAllText(LogFile, line);
            }
        }
        catch { /* logging must never throw */ }
    }
}
