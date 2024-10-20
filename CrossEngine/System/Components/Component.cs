using CrossEngine.System.Components;

namespace CrossEngine.System
{
    public abstract class Component : IComponentble
    {
        public virtual bool Enabled { get; set; } = true;

        public GameObject GameObject { get; }
        public Transform Transform => GameObject.Transform;

        public Component()
        {
            GameObject gameObject = GameObject.gameObject ?? throw new CrossException("Use 'GameObject.AddComponent<>()' for creating new Component!!!");

            GameObject = gameObject;
        }
        public void AddComponent<TComponent>() where TComponent : Component, new()
        {
            GameObject.AddComponent<TComponent>();
        }
        public TComponent GetComponent<TComponent>() where TComponent : Component, new()
        {
            return GameObject.GetComponent<TComponent>();
        }
        public bool TryGetComponent<TComponent>(out TComponent? crossBehaviour) where TComponent : Component, new()
        {
            return GameObject.TryGetComponent(out crossBehaviour);
        }
        public Component[] GetComponents() => GameObject.GetComponents();

        public T[] GetComponents<T>() where T : Component
        {
            return GameObject.GetComponents<T>();
        }
    }
}
