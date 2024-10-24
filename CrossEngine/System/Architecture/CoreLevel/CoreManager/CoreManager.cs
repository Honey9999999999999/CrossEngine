namespace CrossEngine.System.Kernel
{
    internal class CoreManager : Singleton<CoreManager>
    {
        public static event Action? OnStartLoading;
        public static event Action? OnLoaded;

        public static bool IsLoading { get; private set; }

        private readonly Dictionary<Type, CoreLoaderConfig> _coreConfigsMap = new()
        {
            [typeof(ConsoleCoreConfig)] = new ConsoleCoreConfig(),
            [typeof(FormCoreConfig)] = new FormCoreConfig()
        };
        private CoreLoaderConfig? _currentConfig;

        public static void LoadCoreWithConfig<TCoreConfig>(TCoreConfig coreConfig) where TCoreConfig : Type
        {
            Instance._currentConfig = Instance._coreConfigsMap[coreConfig];
            IsLoading = true;
            OnStartLoading?.Invoke();

            Console.WriteLine($"{coreConfig} is start loading...\n");

            CoreLoader.LoadCore(Instance._coreConfigsMap[coreConfig]);

            IsLoading = false;
            OnLoaded?.Invoke();

            Console.WriteLine($"\n{coreConfig} is loaded.\n");
        }

        public static TCoreComponent GetCoreComponent<TCoreComponent>() where TCoreComponent : IInitializeble, new()
        {
            if(Instance._currentConfig is null) throw new CrossException("Core config is not defined.");

            return Instance._currentConfig.GetCoreComponent<TCoreComponent>();
        }
    }
}
