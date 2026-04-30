namespace WarThunderStreamDeckPlugin.Services;

public interface IWarThunderStateService
{
    Task<bool?> GetGearDownAsync(CancellationToken ct = default);
    Task<bool?> GetFlapsExtendedAsync(CancellationToken ct = default);
}
