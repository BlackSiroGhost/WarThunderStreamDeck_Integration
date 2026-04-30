namespace WarThunderStreamDeckPlugin.Actions;

using WarThunderStreamDeckPlugin.Services;

public class ActionFactory : IActionFactory
{
    public static IActionFactory Current { get; set; } = new ActionFactory();

    public IWarThunderStateService CreateStateService() => new WarThunderStateService();
    public IKeyboardSimulator CreateKeyboardSimulator() => new WindowsKeyboardSimulator();
    public IKeyBindingProvider CreateKeyBindingProvider() => new BlkKeyBindingProvider();
}
