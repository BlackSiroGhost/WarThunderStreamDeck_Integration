namespace WarThunderStreamDeckPlugin.Settings;

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

public static class BlkFilePicker
{
    public static string? Pick(string? initialPath)
    {
        string? result = null;
        var thread = new Thread(() =>
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Select War Thunder controls .blk",
                Filter = "War Thunder controls (*.blk)|*.blk|All files (*.*)|*.*",
                CheckFileExists = true,
                Multiselect = false,
                RestoreDirectory = true
            };

            var initialDir = ResolveInitialDirectory(initialPath);
            if (!string.IsNullOrEmpty(initialDir)) dlg.InitialDirectory = initialDir;

            if (dlg.ShowDialog() == DialogResult.OK)
                result = dlg.FileName;
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.IsBackground = true;
        thread.Start();
        thread.Join();
        return result;
    }

    private static string? ResolveInitialDirectory(string? initialPath)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(initialPath))
            {
                var expanded = Environment.ExpandEnvironmentVariables(initialPath);
                var dir = File.Exists(expanded)
                    ? Path.GetDirectoryName(expanded)
                    : Directory.Exists(expanded) ? expanded : null;
                if (!string.IsNullOrEmpty(dir)) return dir;
            }

            var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var saves = Path.Combine(docs, "My Games", "WarThunder", "Saves");
            if (Directory.Exists(saves)) return saves;
        }
        catch { }
        return null;
    }
}
