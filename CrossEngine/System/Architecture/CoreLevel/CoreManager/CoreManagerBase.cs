using CrossEngine.System.Arhitecture.SceneLevel;
using System.Collections;

namespace CrossEngine.System.Arhitecture.CoreLevel.CoreManager
{
    public abstract class CoreManagerBase
    {
        public event Action OnStartLoading;
        public event Action OnLoaded;

        public Core core { get; private set; }
        public TypeConfigs currentConfig { get; private set; }
        public bool isLoading { get; private set; }

        protected Dictionary<TypeConfigs, CoreComponentsConfig> _coreConfigsMap;

        public CoreManagerBase()
        {
            _coreConfigsMap = new();
        }

        public abstract void InitCoreComponentsConfigMap();

        public void LoadCoreEngineWithCurrentConfig()
        {
            LoadCoreEngineWithConfig(currentConfig);
        }

        public void LoadCoreEngineWithConfig(TypeConfigs type)
        {
            isLoading = true;
            OnStartLoading?.Invoke();

            IEnumerator loadRoutine = LoadCoreEngineWithConfigRoutine(type);
            while (loadRoutine.MoveNext()) ;
            currentConfig = type;

            isLoading = false;
            OnLoaded?.Invoke();
        }

        private IEnumerator LoadCoreEngineWithConfigRoutine(TypeConfigs type)
        {
            core = new Core(_coreConfigsMap[type]);

            core.InitializeCore();

            yield return null;
        }

        public void RunCoreEngine()
        {
            core.Run();
        }
    }
}
