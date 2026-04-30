namespace WarThunderStreamDeckPlugin.Services;

using System.Runtime.InteropServices;

public class WindowsKeyboardSimulator : IKeyboardSimulator
{
    [DllImport("user32.dll", SetLastError = true)]
    private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

    private const int KEYEVENTF_KEYDOWN = 0x0000;
    private const int KEYEVENTF_KEYUP = 0x0002;

    public void PressVirtualKey(byte vk)
    {
        keybd_event(vk, 0, KEYEVENTF_KEYDOWN, 0);
        keybd_event(vk, 0, KEYEVENTF_KEYUP, 0);
    }
}
