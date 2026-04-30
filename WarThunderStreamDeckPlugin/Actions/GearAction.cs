namespace WarThunderStreamDeckPlugin.Actions;

using SharpDeck;
using SharpDeck.Events.Received;
using WarThunderStreamDeckPlugin.Services;

[StreamDeckAction("com.myplugin.wtgear.toggle")]
public class GearAction : WarThunderActionBase
{
    public GearAction() : base() { }

    public override string ActionUuid => "com.myplugin.wtgear.toggle";
    public override string DisplayName => "Gear Toggle";
    public override string BindingName => "gear";

    protected override async Task OnKeyDown(ActionEventArgs<KeyPayload> args)
    {
        var vk = KeyBindingProvider.GetVirtualKeyForAction(BindingName, ResolveControlsPath(args));
        Keyboard.PressVirtualKey(vk ?? (byte)'G');

        var gearDown = await StateService.GetGearDownAsync();
        await SetStateAsync(gearDown == true ? 1 : 0);
    }

    protected override async Task OnWillAppear(ActionEventArgs<AppearancePayload> args)
    {
        var gearDown = await StateService.GetGearDownAsync();
        await SetStateAsync(gearDown == true ? 1 : 0);
    }
}
