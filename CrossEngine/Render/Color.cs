using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace CrossEngine.Render
{
    /// <summary> ARGB color representation </summary>
    /// <param name="r"> Red channel </param>
    /// <param name="g"> Green channel </param>
    /// <param name="b"> Blue channel </param>
    /// <param name="a"> Alpha (Opacity) channel </param>
    [StructLayout(LayoutKind.Sequential), Serializable]
    public partial struct Color(byte r, byte g, byte b, byte a = byte.MaxValue)
    {
        private byte a = a, r = r, g = g, b = b;

        /// <summary> Alpha (Opacity) channel </summary>
        public float A { readonly get => BtF(a); set => a = FtB(value); }
        /// <summary> Red channel </summary>
        public float R { readonly get => BtF(r); set => r = FtB(value); }
        /// <summary> Green channel </summary>
        public float G { readonly get => BtF(g); set => g = FtB(value); }
        /// <summary> Blue channel </summary>
        public float B { readonly get => BtF(b); set => b = FtB(value); }

        public readonly float Hue { get => GetHue(); }
        public readonly float Saturation { get => GetHue(); }
        public readonly float Value { get => GetHue(); }

        /// <param name="red"> Red channel </param>
        /// <param name="green"> Green channel </param>
        /// <param name="blue"> Blue channel </param>
        /// <param name="alpha"> Alpha (Opacity) channel </param>
        public Color(float red, float green, float blue, float alpha = 1.0f) : this(FtB(red), FtB(green), FtB(blue), FtB(alpha)) { }

        /// <param name="red"> Red channel </param>
        /// <param name="green"> Green channel </param>
        /// <param name="blue"> Blue channel </param>
        /// <param name="alpha"> Alpha (Opacity) channel </param>
        public Color(int red, int green, int blue, int alpha = byte.MaxValue) : this(ItB(red), ItB(green), ItB(blue), ItB(alpha)) { }



        [GeneratedRegex(@"^#?([a-fA-F0-9]{8}|[a-fA-F0-9]{6}|[a-fA-F0-9]{4}|[a-fA-F0-9]{3})$")]
        private static partial Regex HexRegex();
        private static readonly Regex Hex = HexRegex();
        public static Color FromHex(string text) => !Hex.IsMatch(text) ? default :
            (text = text.TrimStart('#')).Length switch
            {
                3 => new(HexToByte(text[0], text[0]),
                         HexToByte(text[1], text[1]),
                         HexToByte(text[2], text[2])),

                4 => new(HexToByte(text[0], text[0]),
                         HexToByte(text[1], text[1]),
                         HexToByte(text[2], text[2]),
                         HexToByte(text[3], text[3])),

                6 => new(HexToByte(text[0..2]),
                         HexToByte(text[2..4]),
                         HexToByte(text[4..6])),

                8 => new(HexToByte(text[0..2]),
                         HexToByte(text[2..4]),
                         HexToByte(text[4..6]),
                         HexToByte(text[6..8])),

                _ => default
            };

        /// <summary> <see langword="byte"/> to <see langword="float"/> conversion </summary>
        /// <returns> <paramref name="value"/> / 255.0 </returns>
        private static float BtF(byte value) => value * btf;
        private const float btf = 1.0f / byte.MaxValue;

        /// <summary> <see langword="float"/> to <see langword="byte"/> conversion </summary>
        /// <returns> <paramref name="value"/> * 255.0 </returns>
        private static byte FtB(float value) => value switch
        {
            >= 1 => byte.MaxValue,
            <= 0 => byte.MinValue,
            _ => (byte)MathF.Round(value * ftb)
        };
        private const float ftb = byte.MaxValue;

        /// <summary> <see langword="float"/> to <see langword="byte"/> conversion </summary>
        /// <returns> <paramref name="value"/> * 255.0 </returns>
        private static byte ItB(int value) => value switch
        {
            >= byte.MaxValue => byte.MaxValue,
            <= byte.MinValue => byte.MinValue,
            _ => (byte)value
        };

        public static Color Add(Color a, Color b) => a + b;
        public static Color Sub(Color a, Color b) => a - b;
        public static Color Mul(Color a, Color b) => a * b;
        public static Color Div(Color a, Color b) => a / b;
        public static Color Xor(Color a, Color b) => a ^ b;
        public static Color And(Color a, Color b) => a & b;
        public static Color Or(Color a, Color b) => a | b;

        public static Color Lerp(Color a, Color b, float t) => a + (b - a) * t;
        public static Color Mix(Color a, Color b, float t) =>
            FromHSV(a.Hue + (b.Hue - a.Hue) * t,
                    a.Saturation + (b.Saturation - a.Saturation) * t,
                    a.Value + (b.Value - a.Value) * t,
                    a.A + (b.A - a.A) * t);

        public static Color operator +(Color a, Color b) => new(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a); // Sum
        public static Color operator -(Color a, Color b) => new(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a); // Sub
        public static Color operator *(Color a, Color b) => new(a.R * b.R, a.G * b.G, a.B * b.B, a.A * b.A); // Mul (in floats)
        public static Color operator /(Color a, Color b) => new(a.R / b.R, a.G / b.G, a.B / b.B, a.A / b.A); // Div (in floats)
        public static Color operator ^(Color a, Color b) => new(a.r ^ b.r, a.g ^ b.g, a.b ^ b.b, a.a ^ b.a); // XOR
        public static Color operator &(Color a, Color b) => new(a.r & b.r, a.g & b.g, a.b & b.b, a.a & b.a); // AND
        public static Color operator |(Color a, Color b) => new(a.r | b.r, a.g | b.g, a.b | b.b, a.a | b.a); // OR
        public static Color operator *(Color a, float value) => new(a.R * value, a.G * value, a.B * value, a.A * value); // Mul (in floats)
        public static Color operator /(Color a, float value) => new(a.R / value, a.G / value, a.B / value, a.A / value); // Div (in floats)

        public static Color FromRgb(float r, float g, float b) => new(r, g, b);
        public static Color FromArgb(float a, float r, float g, float b) => new(r, g, b, a);

        public static Color FromRgb(byte r, byte g, byte b) => new(r, g, b, byte.MaxValue);
        public static Color FromArgb(byte a, byte r, byte g, byte b) => new(r, g, b, a);

        public static Color FromHSV(float hue, float saturation, float value, float alpha = 1)
        {
            float C = (value = Clamp(value)) * Clamp(saturation),
                  H = hue % MathF.Tau / (MathF.PI / 3),
                  X = C * (1f - MathF.Abs(H % 2 - 1)),
                  m = value - C + 0.5f * btf;

            C = Clamp(C + m);
            X = Clamp(X + m);

            return H switch
            {
                <= 1 => new(alpha, C, X, m),
                <= 2 => new(alpha, X, C, m),
                <= 3 => new(alpha, m, C, X),
                <= 4 => new(alpha, m, X, C),
                <= 5 => new(alpha, X, m, C),
                <= 6 => new(alpha, C, m, X),
                _ => new(alpha, m, m, m)
            };
        }

        private readonly float GetHue()
        {
            float max = Max(r, g, b),
                  min = Min(r, g, b),
                  delta = MathF.PI / (max - min);

            if (max == min) return 0;
            if (max == r) return (g - b) * delta + (g < b ? 360 : 0);
            if (max == g) return (b - r) * delta + 120;
            if (max == b) return (r - g) * delta + 240;

            return 0;
        }

        private static byte HexToByte(string s) => s.Length switch
        {
            1 => HexToByte(s[0]),
            2 => HexToByte(s[0], s[1]),
            _ => 0
        };

        private static byte HexToByte(char a, char b) =>
            (byte)((HexToByte(a) << 4) | HexToByte(b));

        private static byte HexToByte(char c) => (byte)(
              c >= '0' && c <= '9' ? c - '0'
            : c >= 'A' && c <= 'F' ? c - 'A' + 10
            : c >= 'a' && c <= 'f' ? c - 'a' + 10 : 0x10);

        private static float Max(params float[] values) => values.Max();
        private static float Min(params float[] values) => values.Min();
        private static float Clamp(float value, float minimum = 0.0f, float maximum = 1.0f) =>
            value < minimum ? minimum :
            value > maximum ? maximum : value;

        private enum ColorAttributes : byte
        {
            R = 0b0100, // Red
            G = 0b0010, // Green
            B = 0b0001, // Blue
            I = 0b1000  // Intense
        }

        public readonly byte ToConsoleColor()
        {
            byte color = 0b0000; // Black

            byte r = FtB(R * A), // apply alpha
                 g = FtB(G * A),
                 b = FtB(B * A);

            if (r + g + b >= byte.MaxValue * 3 / 2) // bright color
                color |= (byte)ColorAttributes.I;

            if (r * 2 >= byte.MaxValue) color |= (byte)ColorAttributes.R;
            if (g * 2 >= byte.MaxValue) color |= (byte)ColorAttributes.G;
            if (b * 2 >= byte.MaxValue) color |= (byte)ColorAttributes.B;

            return color;
        }

        /// <returns> Hexadecimal #AARRGGBB </returns>
        public override readonly string ToString() => $"#{a:##}{r:##}{g:##}{b:##}";
    }
}
