namespace WarThunderStreamDeckPlugin.Services;

public class WarThunderStateService : IWarThunderStateService
{
    private readonly HttpClient _httpClient;

    public WarThunderStateService(HttpClient? httpClient = null)
    {
        _httpClient = httpClient ?? new HttpClient();
    }

    public async Task<bool?> GetGearDownAsync(CancellationToken ct = default)
    {
        var json = await SafeGetStateAsync(ct);
        return json?.Contains("\"gear\":true") == true ? true : json is null ? null : false;
    }

    public async Task<bool?> GetFlapsExtendedAsync(CancellationToken ct = default)
    {
        var json = await SafeGetStateAsync(ct);
        return json?.Contains("\"flaps\":true") == true ? true : json is null ? null : false;
    }

    private async Task<string?> SafeGetStateAsync(CancellationToken ct)
    {
        try
        {
            return await _httpClient.GetStringAsync("http://localhost:8111/state", ct);
        }
        catch
        {
            return null;
        }
    }
}
