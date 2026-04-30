namespace WarThunderStreamDeckPlugin.Actions;

using SharpDeck;
using SharpDeck.Events.Received;
using WarThunderStreamDeckPlugin.Services;

public interface IWarThunderAction
{
    string ActionUuid { get; }
    string DisplayName { get; }
    // Logischer Bindungsname in der Steuerungsdatei, z. B. "gear", "flaps".
    string BindingName { get; }
}
