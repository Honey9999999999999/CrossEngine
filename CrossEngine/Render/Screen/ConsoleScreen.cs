namespace CrossEngine.Render
{
    public readonly struct ConsoleScreen : IScreen
    {
        public readonly int Width { get; }
        public readonly int Height { get; }

        public readonly float AspectRatio { get; }
        public readonly float SymbolAspectRatio { get; }

        public ConsoleScreen(int width, int height, int symbolWidth, int symbolHeight)
        {
            this.Width = width;
            this.Height = height;

            AspectRatio = (float)width / height;
            SymbolAspectRatio = (float)symbolWidth / symbolHeight;

            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            ConsoleOutput.Setup(ConsoleOutput.GetStdHandle(-11), new(Width, Height));
        }
    }
}
