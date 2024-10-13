namespace CrossEngine
{
    public interface ICrossBehaviour : IInitializeble
    {
        public void Awake();
        public void Start();

        public void Update();
        public void FixedUpdate();

        public void AddComponent<TCrossBehaviour>() where TCrossBehaviour : ICrossBehaviour, new();
    }
}
