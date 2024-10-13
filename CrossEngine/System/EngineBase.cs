using CrossEngine.System.Architecture.Scene;
using CrossEngine.System.Arhitecture.Core;
using System.Threading.Channels;

namespace CrossEngine.System
{
    public abstract class EngineBase<TCoreManager> : IEngine where TCoreManager : CoreManagerBase, new()
    {
        public TCoreManager coreManager { get; private set; }

        public static EngineBase<TCoreManager> instance { get; private set; }

        public TypeConfigs currentConfig { get; private set; }

        public EngineBase() : this(TypeConfigs.ConsoleCore) { }
        public EngineBase(TypeConfigs type)
        {
            if(instance != null)
            {
                throw new CrossException("Engine was be initialized.");
            }
            else
            {
                instance = this;
                currentConfig = type;
                coreManager = new TCoreManager();
            }
        }

        public void StartCore()
        {
            coreManager.InitCoreComponentsConfigMap();

            coreManager.LoadCoreEngineWithCurrentConfig();
        }

        public void RunPlayMode()
        {
            SceneManagerBase.instance.OnSceneLoaded += () => Console.WriteLine($"Scene '{SceneManagerBase.instance.GetActiveScene().name}' is loaded!");

            SceneManagerBase.instance.CreateScene("First Scene");
            SceneManagerBase.instance.LoadScene("First Scene");

            coreManager.core.RunPlayMode();
        }
        public void StopPlayMode()
        {
            coreManager.core.StopRunPlayMode();
        }
    }
}
