using CrossEngine.System;
using CrossEngine.System.Kernel;
using System.Collections;

namespace CrossEngine
{
    public abstract class CrossBehaviour : Component, ICoroutineble
    {
        public override bool Enabled
        {
            get => base.Enabled; set
            {
                base.Enabled = value;
                TryInitialize();
            }
        }

        private List<IEnumerator> _routines;

        private bool isBeAwake;
        private bool isBeOnEnable;
        private bool isBeStart;



        public CrossBehaviour() : base()
        {
            _routines = [];
            GameObject.OnEnable += TryInitialize;
        }



        private void TryInitialize()
        {
            if (Core.isRunPlayMode && GameObject.Enabled && Enabled)
            {
                if (!isBeAwake) Awake();
                if (!isBeOnEnable) OnEnable();
                if (!isBeStart) Start();
            }
        }

        public virtual void Awake() => isBeAwake = true;
        public virtual void OnEnable() => isBeOnEnable = true;
        public virtual void Start() => isBeStart = true;


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
            void StartRoutine() => _routines.Add(routine);

            Core.CoreRequiest(StartRoutine);
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
