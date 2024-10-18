namespace CrossEngine.System
{
    public abstract class Component
    {
        public bool Enabled { get; }

        public GameObject GameObject { get; }
        public Transform Transform { get; }

        public Component()
        {
            GameObject gameObject = GameObject.gameObject ?? throw new CrossException("Use 'GameObject.AddComponent<>()' for creating new Component!!!");

            Enabled = gameObject.Enabled;
            GameObject = gameObject;
            Transform = gameObject.Transform;
        }
    }
}
