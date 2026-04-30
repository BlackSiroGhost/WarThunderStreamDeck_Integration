namespace WarThunderStreamDeckPlugin.Actions;

using System.Threading.Tasks;
using SharpDeck;
using WarThunderStreamDeckPlugin.KeyBindings;
using WarThunderStreamDeckPlugin.Telemetry;

// Sweep cycle: each press extends one notch until flaps hit 100%, then each press
// retracts a notch until they hit 0%, then it sweeps the other way again.
// The icon previews the next press: state 0 = "next extends", state 1 = "next retracts".
//
// OnPressAsync uses the cached percent updated by the 1Hz poll loop so that
// spam-pressing the button never has to wait on a /state HTTP call.
[StreamDeckAction("com.blacksiroghost.wt.mechanisation.flaps")]
public sealed class FlapsAction : WarThunderBindingAction
{
    private const double UpperLimit = 99.5;
    private const double LowerLimit = 0.5;

    private volatile bool _extending = true;
    private double _cachedPercent;

    protected override string TitlePrefix => "FLAPS";

    protected override async Task<TelemetryReading?> ProbeAsync()
    {
        var pct = await Telemetry.GetFlapsPercentAsync().ConfigureAwait(false);
        if (pct is null) return null;

        _cachedPercent = pct.Value;
        UpdateDirection(_cachedPercent);
        return new TelemetryReading(State: _extending ? 0 : 1, Percent: pct);
    }

    protected override Task OnPressAsync(BindingMap bindings)
    {
        UpdateDirection(_cachedPercent);
        var bindingId = _extending ? "ID_FLAPS_DOWN" : "ID_FLAPS_UP";
        return TryFireBindingAsync(bindings, bindingId);
    }

    private void UpdateDirection(double pct)
    {
        if (pct >= UpperLimit) _extending = false;
        else if (pct <= LowerLimit) _extending = true;
    }
}
