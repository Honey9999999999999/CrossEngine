using CrossEngine.System.Kernel;

namespace CrossEngine.System
{
    internal class SceneTree : CoreComponent<SceneTree>
    {
        public static event Action? OnTreeUpdated;

        public Branch Trunck => _trunck ?? throw new CrossException("SceneTree.Trunck is not initialized!!!");
        private Branch? _trunck;

        public Branch[] VisibleBranches => _visibleBranches;
        public Branch[] _visibleBranches = [];

        public Branch ActiveBranch => _activeBranch ?? throw new CrossException("SceneTree.ActiveBranch is not initialized!!!");
        public Branch? _activeBranch;

        public override void Initialize()
        {
            _trunck = new(SceneManager.GetActiveScene().RootNode);
            UpdateVisibleBranches();
            _activeBranch = _visibleBranches[0];
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
            UpdateVisibleBranches();
            OnTreeUpdated?.Invoke();
        }

        private void UpdateVisibleBranches()
        {
            _visibleBranches = GetVisibleBranches(Trunck);
        }

        private Branch[] GetVisibleBranches(Branch Trunck)
        {
            Branch[] branches = [Trunck];

            if(Trunck.IsParent && Trunck.IsOpen)
            {
                foreach (Branch branch in Trunck.Branches)
                {
                    Branch[] childBranches = GetVisibleBranches(branch);
                    int oldLenght = branches.Length;
                    Array.Resize(ref branches, branches.Length + childBranches.Length);
                    Array.Copy(childBranches, 0, branches, oldLenght, childBranches.Length);
                }
            }

            return branches;
        }
    }
}
