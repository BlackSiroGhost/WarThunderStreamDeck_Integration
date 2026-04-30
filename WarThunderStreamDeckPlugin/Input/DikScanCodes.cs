namespace WarThunderStreamDeckPlugin.Input;

using System.Collections.Generic;

// DirectInput DIK scan codes used by Dagor/War Thunder .blk files.
// These are PS/2 set-1 scan codes; high byte 0xE0 is encoded as code+128.
internal static class DikScanCodes
{
    // Codes >= 128 are extended keys (originally 0xE0-prefixed in PS/2 set-1).
    // SendInput needs KEYEVENTF_EXTENDEDKEY for those.
    public static bool IsExtended(int dik) => dik >= 128;

    // Returns the raw 7-bit make code that goes into KEYBDINPUT.wScan.
    public static ushort MakeCode(int dik) => (ushort)(dik & 0x7F);

    // Helpful for users: a human label for the most common DIKs.
    public static string Label(int dik) => Names.TryGetValue(dik, out var n) ? n : $"DIK_{dik}";

    private static readonly Dictionary<int, string> Names = new()
    {
        { 1,  "Esc" },     { 2,  "1" },       { 3,  "2" },       { 4,  "3" },
        { 5,  "4" },       { 6,  "5" },       { 7,  "6" },       { 8,  "7" },
        { 9,  "8" },       { 10, "9" },       { 11, "0" },       { 12, "-" },
        { 13, "=" },       { 14, "Backspace" },{ 15, "Tab" },
        { 16, "Q" },       { 17, "W" },       { 18, "E" },       { 19, "R" },
        { 20, "T" },       { 21, "Y" },       { 22, "U" },       { 23, "I" },
        { 24, "O" },       { 25, "P" },       { 26, "[" },       { 27, "]" },
        { 28, "Enter" },   { 29, "LCtrl" },
        { 30, "A" },       { 31, "S" },       { 32, "D" },       { 33, "F" },
        { 34, "G" },       { 35, "H" },       { 36, "J" },       { 37, "K" },
        { 38, "L" },       { 39, ";" },       { 40, "'" },       { 41, "`" },
        { 42, "LShift" },  { 43, "\\" },
        { 44, "Z" },       { 45, "X" },       { 46, "C" },       { 47, "V" },
        { 48, "B" },       { 49, "N" },       { 50, "M" },       { 51, "," },
        { 52, "." },       { 53, "/" },       { 54, "RShift" },  { 55, "Num*" },
        { 56, "LAlt" },    { 57, "Space" },   { 58, "CapsLock" },
        { 59, "F1" },      { 60, "F2" },      { 61, "F3" },      { 62, "F4" },
        { 63, "F5" },      { 64, "F6" },      { 65, "F7" },      { 66, "F8" },
        { 67, "F9" },      { 68, "F10" },     { 87, "F11" },     { 88, "F12" },
        { 69, "NumLock" }, { 70, "ScrollLk" },
        { 71, "Num7" },    { 72, "Num8" },    { 73, "Num9" },    { 74, "Num-" },
        { 75, "Num4" },    { 76, "Num5" },    { 77, "Num6" },    { 78, "Num+" },
        { 79, "Num1" },    { 80, "Num2" },    { 81, "Num3" },    { 82, "Num0" },
        { 83, "Num." },
        { 156, "NumEnter" }, { 157, "RCtrl" }, { 181, "Num/" }, { 184, "RAlt" },
        { 199, "Home" },   { 200, "Up" },     { 201, "PageUp" },
        { 203, "Left" },   { 205, "Right" },
        { 207, "End" },    { 208, "Down" },   { 209, "PageDown" },
        { 210, "Insert" }, { 211, "Delete" },
    };
}
