namespace WarThunderStreamDeckPlugin.Input;

using System.Runtime.InteropServices;
using System.Threading.Tasks;

public sealed class SendInputKeyboard : IKeyboardInput
{
    // Empirically WT needs a ~30ms hold to register the keypress. Async delay
    // releases the worker thread so other actions can fire concurrently.
    private const int HoldMilliseconds = 30;

    public async Task TapScanCodeAsync(int dik)
    {
        var scan = DikScanCodes.MakeCode(dik);
        var extended = DikScanCodes.IsExtended(dik);
        var flagsBase = KEYEVENTF_SCANCODE | (extended ? KEYEVENTF_EXTENDEDKEY : 0u);
        var size = Marshal.SizeOf<INPUT>();

        var down = new[] { MakeKey(scan, flagsBase) };
        var up = new[] { MakeKey(scan, flagsBase | KEYEVENTF_KEYUP) };

        var sentDown = SendInput(1, down, size);
        await Task.Delay(HoldMilliseconds).ConfigureAwait(false);
        var sentUp = SendInput(1, up, size);

        Diagnostics.PluginLog.Info($"  tap dik={dik} ({DikScanCodes.Label(dik)}) scan=0x{scan:X2} down={sentDown} up={sentUp}");
    }

    public async Task TapVirtualKeyAsync(uint vk)
    {
        var size = Marshal.SizeOf<INPUT>();
        var down = new[] { MakeKeyVk((ushort)vk, 0u) };
        var up   = new[] { MakeKeyVk((ushort)vk, KEYEVENTF_KEYUP) };

        var sentDown = SendInput(1, down, size);
        await Task.Delay(HoldMilliseconds).ConfigureAwait(false);
        var sentUp = SendInput(1, up, size);

        Diagnostics.PluginLog.Info($"  tap vk=0x{vk:X2} down={sentDown} up={sentUp}");
    }

    private static INPUT MakeKey(ushort scan, uint flags)
    {
        var inp = default(INPUT);
        inp.type = INPUT_KEYBOARD;
        inp.union.ki.wScan = scan;
        inp.union.ki.dwFlags = flags;
        return inp;
    }

    private static INPUT MakeKeyVk(ushort vk, uint flags)
    {
        var inp = default(INPUT);
        inp.type = INPUT_KEYBOARD;
        inp.union.ki.wVk = vk;
        inp.union.ki.dwFlags = flags;
        return inp;
    }

    private const uint INPUT_KEYBOARD = 1;
    private const uint KEYEVENTF_KEYUP = 0x0002;
    private const uint KEYEVENTF_SCANCODE = 0x0008;
    private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    [StructLayout(LayoutKind.Sequential)]
    private struct INPUT { public uint type; public InputUnion union; }

    [StructLayout(LayoutKind.Explicit)]
    private struct InputUnion
    {
        [FieldOffset(0)] public MOUSEINPUT mi;
        [FieldOffset(0)] public KEYBDINPUT ki;
        [FieldOffset(0)] public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KEYBDINPUT
    {
        public ushort wVk; public ushort wScan;
        public uint dwFlags; public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEINPUT
    {
        public int dx; public int dy;
        public uint mouseData; public uint dwFlags; public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct HARDWAREINPUT
    {
        public uint uMsg; public ushort wParamL; public ushort wParamH;
    }
}
