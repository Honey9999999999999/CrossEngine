using System.Runtime.InteropServices;

namespace CrossEngine.System
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct CharUnion(char unicode)
    {
        [FieldOffset(0)]
        public char UnicodeChar = unicode;
        [FieldOffset(0)]
        public byte AsciiChar;
    }
}
