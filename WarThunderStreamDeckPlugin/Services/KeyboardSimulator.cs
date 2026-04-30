namespace WarThunderStreamDeckPlugin.Services;

using System.Runtime.InteropServices;

public interface IKeyboardSimulator
{
    void PressVirtualKey(byte vk);
}
