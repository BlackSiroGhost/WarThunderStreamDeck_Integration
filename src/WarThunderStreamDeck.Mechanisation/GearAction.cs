namespace WarThunderStreamDeckPlugin.Actions;

using System.Threading.Tasks;
using SharpDeck;
using WarThunderStreamDeckPlugin.KeyBindings;
using WarThunderStreamDeckPlugin.Telemetry;

[StreamDeckAction("com.blacksiroghost.wt.mechanisation.gear")]
public sealed class GearAction : WarThunderBindingAction
{
    protected override string TitlePrefix => "GEAR";

    protected override async Task<TelemetryReading?> ProbeAsync()
    {
        var pct = await Telemetry.GetGearPercentAsync().ConfigureAwait(false);
        if (pct is null) return null;
        return new TelemetryReading(State: pct >= 50 ? 1 : 0, Percent: pct);
    }

    protected override Task OnPressAsync(BindingMap bindings)
        => TryFireBindingAsync(bindings, "ID_GEAR");
}
