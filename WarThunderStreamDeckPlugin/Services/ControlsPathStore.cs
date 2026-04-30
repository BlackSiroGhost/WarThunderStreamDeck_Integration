namespace WarThunderStreamDeckPlugin.Services;

public sealed class ControlsPathStore
{
    private static readonly Lazy<ControlsPathStore> _instance = new(() => new ControlsPathStore());
    public static ControlsPathStore Instance => _instance.Value;

    private string? _path;
    private readonly object _gate = new();

    private static readonly string DefaultDevFileName = "Neue Steuerung mit VKB Stick (PC).blk";

    private ControlsPathStore()
    {
        // Development fallback: try to locate a .blk shipped with the build/output
        try
        {
            var baseDir = AppContext.BaseDirectory;

            // 1) Preferred: the test file copied to output root by the project
            var rootCandidate = Path.Combine(baseDir, DefaultDevFileName);
            if (File.Exists(rootCandidate)) { _path = rootCandidate; return; }

            // 2) Resources/Controls inside output
            var resCandidate = Path.Combine(baseDir, "Resources", "Controls", DefaultDevFileName);
            if (File.Exists(resCandidate)) { _path = resCandidate; return; }

            // 3) Any .blk in output root
            var anyBlk = Directory.EnumerateFiles(baseDir, "*.blk", SearchOption.TopDirectoryOnly).FirstOrDefault();
            if (anyBlk is not null) { _path = anyBlk; return; }

            // 4) Any .blk in output Resources tree
            var anyBlkRes = Directory.Exists(Path.Combine(baseDir, "Resources"))
                ? Directory.EnumerateFiles(Path.Combine(baseDir, "Resources"), "*.blk", SearchOption.AllDirectories).FirstOrDefault()
                : null;
            if (anyBlkRes is not null) { _path = anyBlkRes; return; }
        }
        catch { }
    }

    public void SetPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Invalid path", nameof(path));
        var full = path;
        try
        {
            full = Environment.ExpandEnvironmentVariables(path).Trim().Trim('"');
            if (!Path.IsPathRooted(full)) full = Path.GetFullPath(full);
        }
        catch { }

        if (!File.Exists(full)) throw new FileNotFoundException(".blk file not found", full);
        if (!full.EndsWith(".blk", StringComparison.OrdinalIgnoreCase)) throw new ArgumentException("Path must point to a .blk file", nameof(path));

        lock (_gate)
        {
            _path = full;
        }
    }

    public bool TryGetPath(out string? path)
    {
        lock (_gate)
        {
            path = _path;
            return !string.IsNullOrWhiteSpace(path) && File.Exists(path);
        }
    }

    public string RequirePath()
    {
        lock (_gate)
        {
            if (!string.IsNullOrWhiteSpace(_path) && File.Exists(_path)) return _path!;
        }
        throw new InvalidOperationException("Controls .blk path is not set or file does not exist. Pass the path via program args or ensure the development file exists (it is copied to the output by the project)." );
    }
}
