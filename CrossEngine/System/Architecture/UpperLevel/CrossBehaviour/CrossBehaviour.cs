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
            TryInitialize();
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

        public virtual void Awake()
        {
            foreach (Transform transform in Transform.GetChilds())
            {
                foreach (CrossBehaviour crossBehaviour in transform.GetComponents<CrossBehaviour>())
                {
                    if (crossBehaviour.Enabled)
                        crossBehaviour.Awake();
                }
            }

            isBeAwake = true;
        }
        public virtual void OnEnable()
        {
            foreach (Transform transform in Transform.GetChilds())
            {
                foreach (CrossBehaviour crossBehaviour in transform.GetComponents<CrossBehaviour>())
                {
                    if (crossBehaviour.Enabled)
                        crossBehaviour.OnEnable();
                }
            }

            isBeOnEnable = true;
        }
        public virtual void Start()
        {
            foreach (Transform transform in Transform.GetChilds())
            {
                foreach (CrossBehaviour crossBehaviour in transform.GetComponents<CrossBehaviour>())
                {
                    if (crossBehaviour.Enabled)
                        crossBehaviour.Start();
                }
            }

            isBeStart = true;
        }


        public virtual void Update()
        {
            foreach (Transform transform in Transform.GetChilds())
            {
                foreach (CrossBehaviour crossBehaviour in transform.GetComponents<CrossBehaviour>())
                {
                    crossBehaviour.Update();
                }
            }

            foreach (var routine in _routines)
            {
                if (routine.Current is ICoroutineDelay delay && !delay.Ready) continue;

                routine.MoveNext();
            }
        }
        public virtual void FixedUpdate()
        {
            foreach (Transform transform in Transform.GetChilds())
            {
                foreach (CrossBehaviour crossBehaviour in transform.GetComponents<CrossBehaviour>())
                {
                    crossBehaviour.FixedUpdate();
                }
            }
        }


        public virtual void OnApplicationQuit()
        {
            foreach (Transform transform in Transform.GetChilds())
            {
                foreach (CrossBehaviour crossBehaviour in transform.GetComponents<CrossBehaviour>())
                {
                    crossBehaviour.OnApplicationQuit();
                }
            }
        }
        public virtual void OnDisable()
        {
            foreach (Transform transform in Transform.GetChilds())
            {
                foreach (CrossBehaviour crossBehaviour in transform.GetComponents<CrossBehaviour>())
                {
                    crossBehaviour.OnDisable();
                }
            }
        }
        public virtual void OnDestroy()
        {
            foreach (Transform transform in Transform.GetChilds())
            {
                foreach (CrossBehaviour crossBehaviour in transform.GetComponents<CrossBehaviour>())
                {
                    crossBehaviour.OnDestroy();
                }
            }
        }


        public Coroutine StartCoroutine(IEnumerator routine)
        {
            void StartRoutine() => _routines.Add(routine);

            Core.CoreRequiest(StartRoutine);
            return Coroutine.CreateCoroutine(routine);
        }
        public void StopCoroutine(Coroutine coroutine) => _routines.Remove(coroutine._routine);
    }
}
