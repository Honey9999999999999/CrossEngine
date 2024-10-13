using CrossEngine.System;
using System.Collections;

namespace CrossEngine
{
    public abstract class CrossBehaviour : Initializeble, ICrossBehaviour, ICoroutineble
    {
        private Dictionary<Type, ICrossBehaviour> components;

        public bool enabled => _enabled;
        private bool _enabled;

        private List<IEnumerator> _routines;

        public CrossBehaviour()
        {
            components = [];
            _routines = [];
            _enabled = true;
        }

        public virtual void Awake() { }
        public void OnEnable() { }
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

        public void OnApplicationQuit() { }
        public void OnDisable() { }
        public void OnDestroy() { }        


        public Coroutine StartCoroutine(IEnumerator routine)
        {
            void Add() => _routines.Add(routine);

            Engine.instance.coreManager.core.AddTask(Add);
            return Coroutine.CreateCoroutine(routine);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            _routines.Remove(coroutine._routine);
        }


        public void AddComponent<TCrossBehaviour>() where TCrossBehaviour : ICrossBehaviour, new()
        {
            components[typeof(TCrossBehaviour)] = new TCrossBehaviour();
        }
        public TCrossBehaviour GetComponent<TCrossBehaviour>() where TCrossBehaviour : ICrossBehaviour, new()
        {
            return (TCrossBehaviour)components[typeof(TCrossBehaviour)];
        }
        public bool TryGetComponent<TCrossBehaviour>(out TCrossBehaviour crossBehaviour) where TCrossBehaviour : ICrossBehaviour, new()
        {
            if (components.ContainsKey(typeof(TCrossBehaviour)))
            {
                crossBehaviour = (TCrossBehaviour)components[typeof(TCrossBehaviour)];

                return true;
            }
            else
            {
                crossBehaviour = default;

                return false;
            }
        }
    }
}
