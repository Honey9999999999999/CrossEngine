using CrossEngine.System.Kernel;

namespace CrossEngine.System
{
    internal class SceneTree : CoreComponent<SceneTree>
    {
        public static event Action? OnTreeUpdated;

        public Branch Trunck => _trunck ?? throw new CrossException("SceneTree.Trunck is not initialized!!!");
        private Branch? _trunck;

        public override void Initialize()
        {
            _trunck = new(SceneManager.GetActiveScene().RootNode);
            OnEnable();

            base.Initialize();
        }

        private void OnEnable()
        {
            GameObject.OnGameObjectBeCreated += UpdateBranch;
            GameObject.OnGameObjectBeDestroyed += UpdateBranch;

            SceneManager.OnSceneUploading += OnDisable;
        }

        private void OnDisable()
        {
            GameObject.OnGameObjectBeCreated -= UpdateBranch;
            GameObject.OnGameObjectBeDestroyed -= UpdateBranch;

            SceneManager.OnSceneUploading -= OnDisable;
        }

        private void UpdateBranch()
        {
            _trunck = new(SceneManager.GetActiveScene().RootNode);
            OnTreeUpdated?.Invoke();
        }
    }
}
