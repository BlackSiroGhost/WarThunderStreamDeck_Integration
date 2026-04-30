namespace WarThunderStreamDeckPlugin.Actions;

using SharpDeck;
using SharpDeck.Events.Received;
using WarThunderStreamDeckPlugin.Services;

[StreamDeckAction("com.myplugin.wtflaps.toggle")]
public class FlapsAction : WarThunderActionBase
{
    public FlapsAction() : base() { }

    public override string ActionUuid => "com.myplugin.wtflaps.toggle";
    public override string DisplayName => "Flaps Toggle";
    public override string BindingName => "flaps";

    protected override async Task OnKeyDown(ActionEventArgs<KeyPayload> args)
    {
        var vk = KeyBindingProvider.GetVirtualKeyForAction(BindingName, ResolveControlsPath(args));
        Keyboard.PressVirtualKey(vk ?? (byte)'F');

        var flaps = await StateService.GetFlapsExtendedAsync();
        await SetStateAsync(flaps == true ? 1 : 0);
    }

    protected override async Task OnWillAppear(ActionEventArgs<AppearancePayload> args)
    {
        var flaps = await StateService.GetFlapsExtendedAsync();
        await SetStateAsync(flaps == true ? 1 : 0);
    }
}
