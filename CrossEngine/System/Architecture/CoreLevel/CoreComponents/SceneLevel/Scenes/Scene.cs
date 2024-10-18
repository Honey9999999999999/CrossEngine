using CrossEngine.System.Kernel;

namespace CrossEngine.System
{
    public sealed class Scene
    {
        public string name;
        public int index;

        public List<GameObject> rootObjects;

        public int rootCount => rootObjects.Count;        

        internal Scene()
        {
            name = "Default Scene";
            rootObjects = [];
        }

        public List<GameObject> GetRootGameObjects()
        {
            return rootObjects;
        }

        internal void SetIndex(int index) => this.index = index;

        public void AddRootObject(GameObject rootObject)
        {
            void Add() => rootObjects.Add(rootObject);

            Core.CoreRequiest(Add);
        }
    }
}
