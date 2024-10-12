namespace CrossEngine.Render
{
    public interface IScreen
    {
        public int Width { get; }
        public int Height { get; }
        public float AspectRatio => (float)Width / Height;
    }
}
