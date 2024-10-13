using System.Collections;

namespace CrossEngine
{
    public interface ICrossBehaviour : IInitializeble
    {
        public bool enabled { get; }

        public void Awake();
        public void OnEnable();
        public void Start();

        public void Update();
        public void FixedUpdate();

        public void OnApplicationQuit();
        public void OnDisable();
        public void OnDestroy();

        public Coroutine StartCoroutine(IEnumerator routine);
        public void StopCoroutine(Coroutine coroutine);

        public void AddComponent<TCrossBehaviour>() where TCrossBehaviour : ICrossBehaviour, new();
        public TCrossBehaviour GetComponent<TCrossBehaviour>() where TCrossBehaviour : ICrossBehaviour, new();
        public bool TryGetComponent<TCrossBehaviour>(out TCrossBehaviour crossBehaviour) where TCrossBehaviour : ICrossBehaviour, new();
    }
}
