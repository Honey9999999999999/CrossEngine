using CrossEngine.System;
using CrossEngine.System.Components;
using CrossEngine.System.Kernel;

namespace CrossEngine
{
    public sealed class GameObject : object, IComponentble
    {
        internal event Action? OnEnable;
        internal static GameObject? gameObject;

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;

                if (Core.isRunPlayMode && Enabled)
                {
                    OnEnable?.Invoke();
                }
            }
        }
        public string Name { get; set; }
        public string Tag { get; set; } = "Default";

        public Transform Transform => GetComponent<Transform>();

        private readonly Dictionary<Type, Component> _componentsMap = [];
        private bool _enabled = true;

        public GameObject() : this("GameObject") { }
        public GameObject(string name)
        {
            Name = name;
            AddComponent<Transform>();
            SceneManager.GetActiveScene().AddRootObject(Transform);
        }

        public GameObject(Transform parent) : this("GameObject", parent) { }
        public GameObject(string name, Transform parent)
        {
            Name = name;
            AddComponent<Transform>();
            Transform.Parent = parent;
            parent.AddChild(Transform);
        }

        public void AddComponent<TComponent>() where TComponent : Component, new()
        {
            gameObject = this;
            _componentsMap[typeof(TComponent)] = new TComponent();
            gameObject = null;
        }

        public TComponent GetComponent<TComponent>() where TComponent : Component, new()
        {
            return _componentsMap.ContainsKey(typeof(TComponent))
                ? (TComponent)_componentsMap[typeof(TComponent)]
                : throw new CrossException($"GameObject '{Name}' has't Component '{typeof(TComponent).Name}'");
        }

        public bool TryGetComponent<TComponent>(out TComponent? crossBehaviour) where TComponent : Component, new()
        {
            bool isContains = _componentsMap.ContainsKey(typeof(TComponent));
            crossBehaviour = isContains ? (TComponent)_componentsMap[typeof(TComponent)] : null;
            return isContains;
        }

        public Component[] GetComponents() => [.. _componentsMap.Values];

        public T[] GetComponents<T>() where T : Component
        {
            T[] componentsList = [];

            foreach (var component in _componentsMap.Values)
            {
                if (component is T t)
                {
                    componentsList = [.. componentsList, t];
                }
            }

            return [.. componentsList];
        }
    }
}
