namespace WarThunderStreamDeckPlugin.Telemetry;

using System.Threading;
using System.Threading.Tasks;

public interface IWarThunderTelemetry
{
    Task<double?> GetGearPercentAsync(CancellationToken ct = default);
    Task<double?> GetFlapsPercentAsync(CancellationToken ct = default);
    Task<double?> GetAirbrakePercentAsync(CancellationToken ct = default);
}
