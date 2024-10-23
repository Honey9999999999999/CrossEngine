using CrossEngine.System;
using CrossEngine.System.Components;
using CrossEngine.System.Kernel;

namespace CrossEngine
{
    public sealed class GameObject : object, IComponentble
    {
        internal event Action? OnEnable;
        internal static GameObject? gameObject;
        internal static event Action? OnGameObjectBeCreated;
        internal static event Action? OnGameObjectBeDestroyed;

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
        public GameObject(string name) : this(name, null) { }
        public GameObject(Transform parent) : this("GameObject", parent) { }
        public GameObject(string name, Transform? parent)
        {
            Name = name;
            AddComponent<Transform>();

            if (parent != null)
            {
                parent.AddChild(Transform);
                OnGameObjectBeCreated?.Invoke();
                return;
            }
            if (SceneManager.TryGetActiveScene(out Scene scene))
            {
                scene.RootNode.Transform.AddChild(Transform);
                OnGameObjectBeCreated?.Invoke();
            }
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


        public static GameObject GetGameObjectWithName(string name)
        {
            return TryGetGameObjectWithName(name, out GameObject gameObject, SceneManager.GetActiveScene().RootNode.Transform.GetChilds())
                ? gameObject
                : throw new CrossException($"GameObject with name '{name} is not found.'");
        }
        public static bool TryGetGameObjectWithName(string name, out GameObject gameObject)
        {
            return TryGetGameObjectWithName(name, out gameObject, SceneManager.GetActiveScene().RootNode.Transform.GetChilds());
        }

        private static bool TryGetGameObjectWithName(string name, out GameObject gameObject, Transform[] parent)
        {
            foreach (Transform child in parent)
            {
                if (child.GameObject.Name == name)
                {
                    gameObject = child.GameObject;
                    return true;
                }

                if (TryGetGameObjectWithName(name, out gameObject, child.GetChilds()))
                {
                    return true;
                }
            }

            gameObject = null;
            return false;
        }


        public static GameObject[] GetGameObjectsWithName(string name)
        {
            return GetGameObjectsWithName(name, SceneManager.GetActiveScene().RootNode.Transform.GetChilds());
        }
        private static GameObject[] GetGameObjectsWithName(string name, Transform[] parent)
        {
            GameObject[] gameObjects = [];

            foreach (Transform child in parent)
            {
                if (child.GameObject.Name == name)
                {
                    gameObjects = [.. gameObjects, child.GameObject];
                }

                GameObject[] childGameObjects = GetGameObjectsWithName(name, child.GetChilds());
                foreach (GameObject gameObject in childGameObjects)
                {
                    gameObjects = [.. childGameObjects, gameObject];
                }
            }

            return gameObjects;
        }
    }
}
