namespace WarThunderStreamDeckPlugin.KeyBindings;

using System.Collections.Generic;

public sealed class BindingMap
{
    public static readonly BindingMap Empty = new(new Dictionary<string, List<int>>());

    private readonly IReadOnlyDictionary<string, List<int>> _byId;

    public BindingMap(IReadOnlyDictionary<string, List<int>> byId)
    {
        _byId = byId;
    }

    public IReadOnlyCollection<string> Ids => (IReadOnlyCollection<string>)_byId.Keys;

    public IReadOnlyList<int> GetScanCodes(string id)
        => _byId.TryGetValue(id, out var list) ? list : Array.Empty<int>();

    public int? FirstScanCode(string id)
    {
        var list = GetScanCodes(id);
        return list.Count == 0 ? null : list[0];
    }
}
