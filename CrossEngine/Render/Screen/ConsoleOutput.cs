using CrossEngine.System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace CrossEngine.Render
{
    internal static class ConsoleOutput
    {
        static IntPtr hConsole;
        static Coord bufferSize;
        static Coord bufferCoord;
        static SmallRect writeRegion;
        //public Vector2 Start { get; }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", EntryPoint = "WriteConsoleOutputW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool WriteConsoleOutput(IntPtr hConsoleOutput, CharInfo[] lpBuffer, Coord dwBufferSize, Coord dwBufferCoord, ref SmallRect lpWriteRegion);
        public static void Write(CharInfo[] buffer) => WriteConsoleOutput(hConsole, buffer, bufferSize, bufferCoord, ref writeRegion);
        public static void Write(CharInfo[] buffer, Vector2 bufferSize, Vector2 bufferCoord, ref SmallRect writeRegion) =>
            WriteConsoleOutput(
                    hConsole,
                    buffer,
                    new()
                    {
                        X = (short)bufferSize.X,
                        Y = (short)bufferSize.Y
                    },
                    new()
                    {
                        X = (short)bufferCoord.X,
                        Y = (short)bufferCoord.Y
                    },
                    ref writeRegion
                );

        //[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "ReadConsoleInputW")]
        //static extern bool ReadConsoleInput(IntPtr hConsoleInput, [Out] CharInfo[] lpBuffer, uint nLength, out uint lpNumberOfEventsRead);

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;
        }


        //[StructLayout(LayoutKind.Explicit)]
        //public struct INPUT_RECORD
        //{
        //    [FieldOffset(0)]
        //    public ushort EventType;
        //    [FieldOffset(4)]
        //    public KEY_EVENT_RECORD KeyEvent;
        //    [FieldOffset(4)]
        //    public MOUSE_EVENT_RECORD MouseEvent;
        //    [FieldOffset(4)]
        //    public WINDOW_BUFFER_SIZE_RECORD WindowBufferSizeEvent;
        //    [FieldOffset(4)]
        //    public MENU_EVENT_RECORD MenuEvent;
        //    [FieldOffset(4)]
        //    public FOCUS_EVENT_RECORD FocusEvent;
        //};

        //[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        //public struct KEY_EVENT_RECORD
        //{
        //    [FieldOffset(0), MarshalAs(UnmanagedType.Bool)]
        //    public bool bKeyDown;
        //    [FieldOffset(4), MarshalAs(UnmanagedType.U2)]
        //    public ushort wRepeatCount;
        //    [FieldOffset(6), MarshalAs(UnmanagedType.U2)]
        //    public VirtualKeys wVirtualKeyCode;
        //    [FieldOffset(8), MarshalAs(UnmanagedType.U2)]
        //    public ushort wVirtualScanCode;
        //    [FieldOffset(10)]
        //    public char UnicodeChar;
        //    [FieldOffset(12), MarshalAs(UnmanagedType.U4)]
        //    public ControlKeyState dwControlKeyState;
        //}

        //// dwControlKeyState bitmask
        //[Flags]
        //public enum ControlKeyState
        //{
        //    RIGHT_ALT_PRESSED = 0x1,
        //    LEFT_ALT_PRESSED = 0x2,
        //    RIGHT_CTRL_PRESSED = 0x4,
        //    LEFT_CTRL_PRESSED = 0x8,
        //    SHIFT_PRESSED = 0x10,
        //    NUMLOCK_ON = 0x20,
        //    SCROLLLOCK_ON = 0x40,
        //    CAPSLOCK_ON = 0x80,
        //    ENHANCED_KEY = 0x100
        //}


        public static void Setup(IntPtr _hConsole, Vector2 size, Vector2 start = default)
        {
            hConsole = _hConsole;
            bufferSize = new()
            {
                X = (short)size.X,
                Y = (short)size.Y
            };

            bufferCoord = new Coord
            {
                X = (short)start.X,
                Y = (short)start.Y
            };

            writeRegion = new SmallRect
            {
                Left = (short)start.X,
                Top = (short)start.Y,
                Right = (short)size.X,
                Bottom = (short)size.Y
            };
        }
    }
}
