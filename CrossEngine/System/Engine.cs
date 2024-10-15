using CrossEngine.System.Core;

namespace CrossEngine.System
{
    internal sealed class Engine : Singleton<Engine>, IEngine
    {
        public CoreManagerBase coreManager { get; private set; }

        public static TypeConfigs currentConfig { get; private set; }

        public Engine() : this(TypeConfigs.ConsoleCore) { }
        public Engine(TypeConfigs type)
        {
            currentConfig = type;
            coreManager = new CoreManagerExample();
        }

        public static void StartCore()
        {
            _instance.coreManager.InitCoreComponentsConfigMap();

            _instance.coreManager.LoadCoreEngineWithCurrentConfig();
        }

        public static ICore GetCore()
        {
            return _instance.coreManager.core;
        }

        public static void RunPlayMode()
        {
            _instance.coreManager.core.RunPlayMode();
        }
        public static void StopPlayMode()
        {
            _instance.coreManager.core.StopRunPlayMode();
        }
    }
}
