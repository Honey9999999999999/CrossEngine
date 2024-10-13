namespace CrossEngine.System.Architecture.Scene
{
    internal abstract class SceneBase : IScene
    {
        public string name => _name;
        public int index => _index;
        public bool isLoaded => _isLoaded;
        public int rootCount => _rootObjects.Count;

        private string _name;
        private int _index;
        private bool _isLoaded;

        private List<CrossBehaviour> _rootObjects;

        internal SceneBase(string name)
        {
            _name = name;
            _rootObjects = [];
        }

        public List<CrossBehaviour> GetRootGameObjects()
        {
            return _rootObjects;
        }

        internal void SetIndex(int index) => _index = index;

        public void AddRootObject(CrossBehaviour rootObject)
        {
            _rootObjects.Add(rootObject);
        }
    }
}
