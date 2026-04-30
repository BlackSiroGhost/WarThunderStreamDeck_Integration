namespace WarThunderStreamDeckPlugin.Actions;

using System.Threading.Tasks;
using SharpDeck;
using WarThunderStreamDeckPlugin.KeyBindings;
using WarThunderStreamDeckPlugin.Telemetry;

[StreamDeckAction("com.blacksiroghost.wt.mechanisation.airbrake")]
public sealed class AirBrakeAction : WarThunderBindingAction
{
    protected override string TitlePrefix => "AIRBRK";

    protected override async Task<TelemetryReading?> ProbeAsync()
    {
        var pct = await Telemetry.GetAirbrakePercentAsync().ConfigureAwait(false);
        if (pct is null) return null;
        return new TelemetryReading(State: pct >= 50 ? 1 : 0, Percent: pct);
    }

    protected override Task OnPressAsync(BindingMap bindings)
        => TryFireBindingAsync(bindings, "ID_AIR_BRAKE");
}
