namespace WarThunderStreamDeckPlugin.Telemetry;

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public sealed class WarThunderTelemetry : IWarThunderTelemetry, IDisposable
{
    private readonly HttpClient _http;
    private readonly bool _ownsHttp;

    public WarThunderTelemetry(HttpClient? http = null)
    {
        _http = http ?? new HttpClient { Timeout = TimeSpan.FromMilliseconds(750) };
        _ownsHttp = http is null;
    }

    public Task<double?> GetGearPercentAsync(CancellationToken ct = default)
        => ReadPercentAsync("gear, %", ct);

    public Task<double?> GetFlapsPercentAsync(CancellationToken ct = default)
        => ReadPercentAsync("flaps, %", ct);

    public Task<double?> GetAirbrakePercentAsync(CancellationToken ct = default)
        => ReadPercentAsync("airbrake, %", ct);

    private async Task<double?> ReadPercentAsync(string field, CancellationToken ct)
    {
        try
        {
            using var stream = await _http.GetStreamAsync("http://127.0.0.1:8111/state", ct).ConfigureAwait(false);
            using var doc = await JsonDocument.ParseAsync(stream, default, ct).ConfigureAwait(false);

            if (!doc.RootElement.TryGetProperty(field, out var value)) return null;
            return value.ValueKind switch
            {
                JsonValueKind.Number => value.GetDouble(),
                JsonValueKind.True => 100,
                JsonValueKind.False => 0,
                _ => null,
            };
        }
        catch
        {
            return null;
        }
    }

    public void Dispose()
    {
        if (_ownsHttp) _http.Dispose();
    }
}
