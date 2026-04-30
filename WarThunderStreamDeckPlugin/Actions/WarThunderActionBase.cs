namespace WarThunderStreamDeckPlugin.Actions;

using SharpDeck;
using SharpDeck.Events.Received;
using WarThunderStreamDeckPlugin.Services;

// Basisklasse für WT-Aktionen
public abstract class WarThunderActionBase : StreamDeckAction, IWarThunderAction
{
    protected IWarThunderStateService StateService { get; }
    protected IKeyboardSimulator Keyboard { get; }
    protected IKeyBindingProvider KeyBindingProvider { get; }

    protected WarThunderActionBase()
    {
        var factory = ActionFactory.Current;
        StateService = factory.CreateStateService();
        Keyboard = factory.CreateKeyboardSimulator();
        KeyBindingProvider = factory.CreateKeyBindingProvider();
    }

    protected WarThunderActionBase(
        IWarThunderStateService stateService,
        IKeyboardSimulator keyboard,
        IKeyBindingProvider keyBindingProvider)
    {
        StateService = stateService;
        Keyboard = keyboard;
        KeyBindingProvider = keyBindingProvider;
    }

    public abstract string ActionUuid { get; }
    public abstract string DisplayName { get; }
    public abstract string BindingName { get; }

    protected string? ResolveControlsPath(ActionEventArgs<KeyPayload>? args)
        => ControlsPathHelper.ResolveFromSettingsOrEnv(args?.Payload?.Settings);

    protected string? ResolveControlsPath(ActionEventArgs<AppearancePayload>? args)
        => ControlsPathHelper.ResolveFromSettingsOrEnv(args?.Payload?.Settings);

    // Standard-Implementierung: nichts tun; abgeleitete Klassen können überschreiben.
    protected override Task OnWillAppear(ActionEventArgs<AppearancePayload> args) => Task.CompletedTask;
}
