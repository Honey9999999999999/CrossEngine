namespace CrossEngine
{
    public class Initializeble : IInitializeble
    {
        public bool isInitialized => _isInitialized;
        private bool _isInitialized;

        public event Action? OnInitialized;

        public virtual void OnCreate() { Console.WriteLine($"{GetType()} is Created"); }
        public virtual void Initialize()
        {
            Console.WriteLine($"{GetType()} is Initialized");
            OnInitialized?.Invoke();
            _isInitialized = true;
        }
    }
}
