using CrossEngine.System;
using CrossEngine.System.Kernel;
using System.Collections;

namespace CrossEngine
{
    public abstract class CrossBehaviour : Component, ICoroutineble
    {
        private List<IEnumerator> _routines;



        public CrossBehaviour()
        {
            _routines = [];
        }



        public virtual void Awake() { }
        public virtual void OnEnable() { }
        public virtual void Start() { }


        public virtual void Update()
        {
            foreach (var routine in _routines)
            {
                if (routine.Current is ICoroutineDelay delay && !delay.Ready) continue;

                routine.MoveNext();
            }
        }
        public virtual void FixedUpdate() { }


        public virtual void OnApplicationQuit() { }
        public virtual void OnDisable() { }
        public virtual void OnDestroy() { }


        public Coroutine StartCoroutine(IEnumerator routine)
        {
            void Start() => _routines.Add(routine);

            Core.CoreRequiest(Start);
            return Coroutine.CreateCoroutine(routine);
        }
        public void StopCoroutine(Coroutine coroutine)
        {
            _routines.Remove(coroutine._routine);
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
            return GameObject.TryGetComponent<TComponent>(out crossBehaviour);
        }       
    }
}
