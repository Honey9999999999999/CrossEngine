namespace CrossEngine.System
{
    internal struct VectorInt2
    {
        public int X { get; set; }
        public int Y { get; set; }

        public VectorInt2() : this(0, 0) { }
        public VectorInt2(int x, int y)
        {
            X = x;
            Y = y;
        }




        public static VectorInt2 operator +(VectorInt2 a, VectorInt2 b) => new VectorInt2(a.X + b.X, a.Y + b.Y);
        public static VectorInt2 operator -(VectorInt2 a, VectorInt2 b) => new VectorInt2(a.X - b.X, a.Y - b.Y);
        public static VectorInt2 operator *(VectorInt2 a, int b) => new VectorInt2(a.X * b, a.Y * b);
        public override string ToString() => $"<{X};{Y}>";
    }
}
