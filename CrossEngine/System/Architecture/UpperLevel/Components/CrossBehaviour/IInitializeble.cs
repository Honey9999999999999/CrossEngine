namespace CrossEngine
{
    public interface IInitializeble
    {
        public event Action? OnInitialized;
        public bool isInitialized { get; }

        public void OnCreate();
        public void Initialize();
    }
}
