using CrossEngine.System.Arhitecture.CoreLevel.CoreManager;

namespace CrossEngine.System
{
    public abstract class EngineBase<TCoreManager> : IEngine where TCoreManager : CoreManagerBase, new()
    {
        public static TCoreManager coreManager { get; private set; }

        public static void Run()
        {
            coreManager = new TCoreManager();

            coreManager.InitCoreComponentsConfigMap();

            coreManager.LoadCoreEngineWithCurrentConfig();

            coreManager.RunCoreEngine();
        }
    }
}
