namespace WarThunderStreamDeckPlugin.Services;

public static class ControlsPathHelper
{
    // Delegate to the central store; legacy wrapper to avoid touching all call sites.
    public static void SetRuntimeOverride(string? path)
    {
        if (string.IsNullOrWhiteSpace(path)) return;
        ControlsPathStore.Instance.SetPath(path);
    }

    public static string? ResolveFromSettingsOrEnv(object? settings)
    {
        // We no longer use settings/env/appsettings. A valid path must be set via args.
        if (ControlsPathStore.Instance.TryGetPath(out var p)) return p;
        try
        {
            // During development, ControlsPathStore has a fallback to Resources/Controls.
            return ControlsPathStore.Instance.RequirePath();
        }
        catch
        {
            return null;
        }
    }
}
