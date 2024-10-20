namespace CrossEngine.System.Kernel
{
    public abstract class CoreComponent<T> : Singleton<T>, IInitializeble where T : Singleton<T>
    {
        public bool isInitialized => _isInitialized;

        private bool _isInitialized;

        public event Action? OnInitialized;

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
