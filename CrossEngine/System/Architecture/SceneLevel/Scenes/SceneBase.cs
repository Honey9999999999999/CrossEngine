using CrossEngine.System.Kernel;

namespace CrossEngine.System
{
    public abstract class SceneBase : IScene
    {
        public string name => _name;
        public int index => _index;
        public int rootCount => _rootObjects.Count;

        private string _name;
        private int _index;

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
            void Add() => _rootObjects.Add(rootObject);

            Core.CoreRequiest(Add);
        }
    }
}
