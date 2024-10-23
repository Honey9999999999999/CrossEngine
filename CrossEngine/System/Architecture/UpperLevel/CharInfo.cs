using System.Runtime.InteropServices;

namespace CrossEngine.System
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct CharInfo
    {
        [FieldOffset(0)]
        public CharUnion Char;
        [FieldOffset(2)]
        public short Attributes;

        public CharInfo() : this(' ', ConsoleColor.White) { }
        public CharInfo(char unicode, ConsoleColor attributes)
        {
            Char = new CharUnion(unicode);
            Attributes = (short)attributes;
        }
    }
}
