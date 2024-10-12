namespace CrossEngine.System.Interfaces
{
    public interface ICrossBehaviour
    {
        public void OnCreate();
        public void Initialize();
        public void Awake();
        public void Start();
        public void Update();
        public void FixedUpdate();
    }
}
