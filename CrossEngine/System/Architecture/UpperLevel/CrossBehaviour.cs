using System.Collections;

namespace CrossEngine
{
    public abstract class CrossBehaviour : Initializeble, ICrossBehaviour, ICoroutineble
    {
        private bool _isInitialized;

        private List<ICrossBehaviour> components;        

        public CrossBehaviour()
        {
            components = [];
        }

        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            if (routine == null)
            {
                throw new NullReferenceException("routine is null");
            }

            CoroutinesBase.instance.StartRoutine(routine);
            return Coroutine.CreateCoroutine(routine);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            CoroutinesBase.instance.StopRoutine(coroutine);
        }

        public void AddComponent<TCrossBehaviour>() where TCrossBehaviour : ICrossBehaviour, new()
        {
            components.Add(new TCrossBehaviour());
        }
    }
}
