using CrossEngine.System.Kernel;

namespace CrossEngine.System
{
    public sealed class Scene
    {
        public string name;
        public int index;

        public List<CrossBehaviour> rootObjects;

        public int rootCount => rootObjects.Count;        

        internal Scene()
        {
            name = "Default Scene";
            rootObjects = [];
        }

        public List<CrossBehaviour> GetRootGameObjects()
        {
            return rootObjects;
        }

        internal void SetIndex(int index) => this.index = index;

        public void AddRootObject(CrossBehaviour rootObject)
        {
            void Add() => rootObjects.Add(rootObject);

            Core.CoreRequiest(Add);
        }
    }
}
