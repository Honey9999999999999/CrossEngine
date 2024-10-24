using CrossEngine.System;
using CrossEngine.System.Kernel;

namespace CrossEngine.Render
{
    public class ConsoleScreen : CoreComponent<ConsoleScreen>, IScreen
    {
        public int Width { get; }
        public int Height { get; }

        public float AspectRatio { get; }
        public float SymbolAspectRatio { get; }

        private const string _fileName = "ConsoleScreenConfiguration";


        public ConsoleScreen()
        {
            FileManager fileManager = Engine.GetCoreComponent<FileManager>();

            ScreenConfiguration configuration = fileManager.IsPathExist(SavePlace.Screen, _fileName)
                ? fileManager.LoadFromXml<ScreenConfiguration>(SavePlace.Screen, _fileName)
                : new ScreenConfiguration();

            Width = configuration.Width;
            Height = configuration.Height;

            AspectRatio = (float)Width / Height;
            SymbolAspectRatio = configuration.SymbolAspectRatio;

            fileManager.SaveInXml(configuration, _fileName, SavePlace.Screen);

            Console.SetWindowSize(Width, Height);
            Console.SetBufferSize(Width, Height);

            ConsoleOutput.Setup(ConsoleOutput.GetStdHandle(-11), new(Width, Height));
        }
    }
}
