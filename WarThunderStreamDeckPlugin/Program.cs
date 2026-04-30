using SharpDeck;
using WarThunderStreamDeckPlugin.Services;
using WarThunderStreamDeckPlugin.Tests;

namespace WarThunderStreamDeckPlugin;

internal class Program
{
    public static Task Main(string[] args)
    {
        // Detect Stream Deck registration args (SharpDeck expects the -port ... style)
        bool isStreamDeckRegistration = args.Any(a => a.Equals("-port", StringComparison.OrdinalIgnoreCase));

        // In dev/console mode (not launched by Stream Deck), allow passing a .blk path as args
        if (!isStreamDeckRegistration && args is { Length: > 0 })
        {
            var candidate = string.Join(" ", args).Trim().Trim('"');
            candidate = Environment.ExpandEnvironmentVariables(candidate);
            try
            {
                if (!Path.IsPathRooted(candidate)) candidate = Path.GetFullPath(candidate);
            }
            catch { }

            try
            {
                ControlsPathStore.Instance.SetPath(candidate);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return Task.CompletedTask;
            }
        }

        // Ensure a path exists (either from args or dev fallback inside output)
        try
        {
            _ = ControlsPathStore.Instance.RequirePath();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}\nProvide a path to a .blk controls file as the first argument when running standalone, or ensure the test .blk is copied to the output.");
            return Task.CompletedTask;
        }

        // Optional test dump: when WT_DUMP_BINDINGS=1, print property->keys and exit (standalone only)
        if (!isStreamDeckRegistration && Environment.GetEnvironmentVariable("WT_DUMP_BINDINGS") == "1")
        {
            BindingsDump.Print();
            return Task.CompletedTask;
        }

        // Only start SharpDeck when launched by the Stream Deck host
        if (isStreamDeckRegistration)
        {
            return StreamDeckPlugin.RunAsync();
        }

        // Standalone run without registration: show help and exit
        Console.Error.WriteLine("Not launched by Stream Deck. Set WT_DUMP_BINDINGS=1 to print mappings or pass a .blk path to validate it.");
        return Task.CompletedTask;
    }
}