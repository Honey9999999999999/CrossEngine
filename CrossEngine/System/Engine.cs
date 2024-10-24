using CrossEngine.System.Kernel;

namespace CrossEngine.System
{
    internal sealed class Engine
    {
        private static Dictionary<TypeConfig, Type> _configsMap = new()
        {
            [TypeConfig.ConsoleCore] = typeof(ConsoleCoreConfig),
            [TypeConfig.FormCore] = typeof(FormCoreConfig)
        };

        private Engine() { }

        public static void Initialize(TypeConfig config)
        {
            _ = new CoreManager();
            _ = new CoreLoader();

            CoreManager.LoadCoreWithConfig(_configsMap[config]);
        }

        public static void RunPlayMode()
        {
            Core.RunPlayMode();
        }
        public static void StopPlayMode()
        {
            Core.StopRunPlayMode();
        }

        public static TCoreComponent GetCoreComponent<TCoreComponent>() where TCoreComponent : IInitializeble, new()
        {
            return CoreManager.GetCoreComponent<TCoreComponent>();
        } 
    }
}
