using CrossEngine.System.Kernel;
using System.Xml.Serialization;

namespace CrossEngine.System
{
    public sealed class Scene
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public int rootCount => _rootObjects.Length;

        private Transform[] _rootObjects;

        internal Scene()
        {
            Name = "Default Scene";
            _rootObjects = [];
        }

        public Transform[] GetRootObjects()
        {
            return _rootObjects;
        }

        internal void SetIndex(int index) => Index = index;

        public void AddRootObject(Transform rootObject)
        {
            void Add() => _rootObjects = [.. _rootObjects, rootObject];

            Core.CoreRequiest(Add);
        }
    }
}
