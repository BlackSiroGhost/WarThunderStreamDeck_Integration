namespace WarThunderStreamDeckPlugin.Input;

using System.Threading.Tasks;

public interface IKeyboardInput
{
    Task TapScanCodeAsync(int dik);
    Task TapVirtualKeyAsync(uint vk);
}
