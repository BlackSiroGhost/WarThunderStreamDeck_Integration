namespace WarThunderStreamDeckPlugin.Services;

// Liefert VK-Codes für Aktionen aus der WT-Steuerungsdatei
public interface IKeyBindingProvider
{
    byte? GetVirtualKeyForAction(string actionName, string? controlsFilePath = null);
}
