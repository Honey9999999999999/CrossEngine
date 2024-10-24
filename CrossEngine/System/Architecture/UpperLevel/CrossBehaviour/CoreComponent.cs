namespace CrossEngine.System.Kernel
{
    public abstract class CoreComponent<T> : IInitializeble where T : class
    {
        public bool isInitialized => _isInitialized;

        private bool _isInitialized;

        public event Action? OnInitialized;

        private static CoreComponent<T>? _instance;

        public CoreComponent()
        {
            if (_instance != null)
            {
                throw new CrossException($"{GetType()} is be initialized!!!");
            }
            _instance = this;
        }

        public virtual void OnCreate()
        {
            Console.WriteLine($"{GetType()} is Created");
        }
        public virtual void Initialize()
        {
            Console.WriteLine($"{GetType()} is Initialized");
            _isInitialized = true;
            OnInitialized?.Invoke();
        }
    }
}
