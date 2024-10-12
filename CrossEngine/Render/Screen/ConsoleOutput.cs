using System.Numerics;
using System.Runtime.InteropServices;

namespace CrossEngine.Render
{
    public static class ConsoleOutput
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

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)]
            public char UnicodeChar;
            [FieldOffset(0)]
            public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)]
            public CharUnion Char;
            [FieldOffset(2)]
            public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

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
