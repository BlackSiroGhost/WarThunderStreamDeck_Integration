namespace WarThunderStreamDeckPlugin.Actions;

using System.Threading.Tasks;
using SharpDeck;
using WarThunderStreamDeckPlugin.Telemetry;

[StreamDeckAction("com.blacksiroghost.warthunder.flaps")]
public sealed class FlapsAction : WarThunderBindingAction
{
    protected override string PrimaryBindingId => "ID_FLAPS";

    protected override string[] FallbackBindingIds { get; } = { "ID_FLAPS_DOWN", "ID_FLAPS_UP" };

    protected override async Task<int?> ProbeStateAsync(IWarThunderTelemetry telemetry)
    {
        var pct = await telemetry.GetFlapsPercentAsync().ConfigureAwait(false);
        return pct is null ? null : pct > 5 ? 1 : 0;
    }
}
