namespace CrossEngine.Render
{
    public class ScreenConfiguration
    {
        public int Width { get; init; }
        public int Height { get; init; }

        public float AspectRatio { get; init; }
        public float SymbolAspectRatio { get; init; }

        public ScreenConfiguration() : this(240, 60, 4, 8) { }
        public ScreenConfiguration(int width, int height, int symbolWidth, int symbolHeight)
        {
            Width = width;
            Height = height;

            AspectRatio = (float)width / height;
            SymbolAspectRatio = (float)symbolWidth / symbolHeight;
        }
    }
}
