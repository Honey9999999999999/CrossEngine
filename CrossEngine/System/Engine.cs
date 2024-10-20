using CrossEngine.System.Kernel;

namespace CrossEngine.System
{
    internal sealed class Engine : Singleton<Engine>
    {
        public static TypeConfig currentConfig { get; private set; }

        public Engine()
        {
            _ = new CoreManager();
            _ = new CoreLoader();
        }

        public static void StartCore(TypeConfig config)
        {
            CoreManager.LoadCoreWithConfig(config);
            currentConfig = config;
        }

        public static void RunPlayMode()
        {
            Core.RunPlayMode();
        }
        public static void StopPlayMode()
        {
            Core.StopRunPlayMode();
        }
    }
}
