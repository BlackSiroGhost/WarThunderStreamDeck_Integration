namespace WarThunderStreamDeckPlugin.Actions;

using WarThunderStreamDeckPlugin.Services;

public interface IActionFactory
{
    IWarThunderStateService CreateStateService();
    IKeyboardSimulator CreateKeyboardSimulator();
    IKeyBindingProvider CreateKeyBindingProvider();
}
