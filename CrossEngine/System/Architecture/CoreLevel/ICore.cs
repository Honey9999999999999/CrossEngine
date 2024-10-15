namespace CrossEngine.System.Core
{
    public interface ICore
    {
        public event Action? OnInitialized;
        public event Action? OnUpdateStarted;
        public event Action? OnPreUpdate;
        public event Action? OnLateUpdate;
        public event Action? OnUpdateStoped;

        public bool isInitialized { get; }
        public bool isRunPlayMode { get; }

        public void InitializeCore();
        public void CoreRequiest(Action action);
        public void RunPlayMode();
        public void StopRunPlayMode();
    }
}
