namespace WarThunderStreamDeckPlugin.Actions;

using System.Threading.Tasks;
using SharpDeck;
using WarThunderStreamDeckPlugin.Telemetry;

[StreamDeckAction("com.blacksiroghost.warthunder.gear")]
public sealed class GearAction : WarThunderBindingAction
{
    protected override string PrimaryBindingId => "ID_GEAR";

    protected override async Task<int?> ProbeStateAsync(IWarThunderTelemetry telemetry)
    {
        var pct = await telemetry.GetGearPercentAsync().ConfigureAwait(false);
        return pct is null ? null : pct > 50 ? 1 : 0;
    }
}
