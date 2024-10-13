namespace CrossEngine.System.Arhitecture.Core
{
    public interface ICore
    {
        public event Action? OnInitialized;
        public bool isInitialized { get; }
        public bool isRunPlayMode { get; }

        public void InitializeCore();
        public void RunPlayMode();
        public void StopRunPlayMode();
    }
}
