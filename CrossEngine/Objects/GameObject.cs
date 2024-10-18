using CrossEngine.System;

namespace CrossEngine
{
    public class GameObject : object
    {
        internal static GameObject? gameObject;

        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string Tag { get; set; } = "Default";

        public Transform Transform { get; } = new();

        private readonly Dictionary<Type, Component> _componentsMap = [];



        public GameObject()
        {
            Name = GetType().Name;
            SceneManager.GetActiveScene().AddRootObject(this);
        }
        public GameObject(string name)
        {
            Name = name;
            SceneManager.GetActiveScene().AddRootObject(this);
        }



        public void AddComponent<TComponent>() where TComponent : Component, new()
        {
            gameObject = this;
            _componentsMap[typeof(TComponent)] = new TComponent();
        }
        public TComponent GetComponent<TComponent>() where TComponent : Component, new()
        {
            return (TComponent)_componentsMap[typeof(TComponent)];
        }
        public bool TryGetComponent<TComponent>(out TComponent? crossBehaviour) where TComponent : Component, new()
        {
            if (_componentsMap.ContainsKey(typeof(TComponent)))
            {
                crossBehaviour = (TComponent)_componentsMap[typeof(TComponent)];

                return true;
            }
            else
            {
                crossBehaviour = null;

                return false;
            }
        }


        public Component[] GetComponents()
        {
            return [.. _componentsMap.Values];
        }
        public T[] GetComponents<T>() where T : Component
        {
            List<T> componentsList = [];

            foreach (var component in _componentsMap.Values)
            {
                if (component is T t)
                {
                    componentsList.Add(t);
                }
            }

            return [.. componentsList];
        }
    }
}
