using System.Collections;

namespace CrossEngine.System.Arhitecture.Core
{
    public abstract class CoreManagerBase
    {
        public event Action? OnStartLoading;
        public event Action? OnLoaded;

        internal Core core
        {
            get
            {
                if (_core == null)
                {
                    throw new CrossException("Core is not initialized!!!");
                }
                else
                {
                    return _core;
                }
            }
        }
        private Core _core;

        public TypeConfigs currentConfig { get; private set; }
        public bool isLoading { get; private set; }

        protected Dictionary<TypeConfigs, CoreComponentsConfig> _coreConfigsMap;

        public CoreManagerBase()
        {
            _coreConfigsMap = [];
            currentConfig = Engine.instance.currentConfig;
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

            Console.WriteLine($"{currentConfig} is start loading...\n");

            IEnumerator loadRoutine = LoadCoreEngineWithConfigRoutine(type);
            while (loadRoutine.MoveNext()) ;
            currentConfig = type;

            isLoading = false;
            OnLoaded?.Invoke();

            Console.WriteLine($"\n{currentConfig} is loaded.\n");
        }

        private IEnumerator LoadCoreEngineWithConfigRoutine(TypeConfigs type)
        {
            _core = new Core(_coreConfigsMap[type]);

            _core.InitializeCore();

            yield return null;
        }
    }
}
