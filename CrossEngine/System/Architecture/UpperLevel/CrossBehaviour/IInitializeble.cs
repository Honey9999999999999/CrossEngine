namespace CrossEngine.System
{
    public interface IInitializeble
    {
        public abstract event Action? OnInitialized;
        public bool isInitialized { get; }

        public void OnCreate();
        public void Initialize();
    }
}
