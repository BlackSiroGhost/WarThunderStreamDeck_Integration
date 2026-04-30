namespace WarThunderStreamDeckPlugin.Settings;

using System;
using System.IO;
using System.Threading;
using WarThunderStreamDeckPlugin.KeyBindings;

// Loads .blk on demand and reloads when the file is replaced. One global instance.
public sealed class BindingsCache
{
    public static BindingsCache Instance { get; } = new();

    private readonly object _gate = new();
    private string? _path;
    private DateTime _lastWriteUtc;
    private BindingMap _map = BindingMap.Empty;

    public void SetPath(string? path)
    {
        var normalized = string.IsNullOrWhiteSpace(path) ? null : path.Trim().Trim('"');
        lock (_gate)
        {
            if (string.Equals(_path, normalized, StringComparison.OrdinalIgnoreCase)) return;
            _path = normalized;
            _lastWriteUtc = default;
            _map = BindingMap.Empty;
        }
    }

    public string? CurrentPath
    {
        get { lock (_gate) return _path; }
    }

    public BindingMap GetMap()
    {
        lock (_gate)
        {
            if (string.IsNullOrWhiteSpace(_path) || !File.Exists(_path))
                return BindingMap.Empty;

            var write = File.GetLastWriteTimeUtc(_path);
            if (write != _lastWriteUtc)
            {
                _map = BlkParser.LoadFromFile(_path);
                _lastWriteUtc = write;
            }
            return _map;
        }
    }
}
