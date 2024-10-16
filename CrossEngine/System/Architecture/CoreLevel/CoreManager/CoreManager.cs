namespace CrossEngine.System.Kernel
{
    public class CoreManager : Singleton<CoreManager>
    {
        public static event Action? OnStartLoading;
        public static event Action? OnLoaded;

        public static bool isLoading { get; private set; }

        private Dictionary<TypeConfig, CoreLoaderConfig> _coreConfigsMap = new()
        {
            [TypeConfig.ConsoleCore] = new ConsoleCoreConfig(),
            [TypeConfig.FormCore] = new FormCoreConfig()
        };

        public static void LoadCoreWithConfig(TypeConfig type)
        {
            isLoading = true;
            OnStartLoading?.Invoke();

            Console.WriteLine($"{type} is start loading...\n");

            CoreLoader.LoadCore(instance._coreConfigsMap[type]);

            isLoading = false;
            OnLoaded?.Invoke();

            Console.WriteLine($"\n{type} is loaded.\n");
        }
    }
}
