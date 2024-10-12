using CrossEngine.System.Arhitecture.UpperLevel;
using CrossEngine.System.Arhitecture.UpperLevel.Components.Coroutines;
using CrossEngine.System.Interfaces;
using System.Collections;

namespace CrossEngine.System
{
    public abstract class CrossBehaviour : ICrossBehaviour, ICoroutineble
    {
        public static event Action OnInitialized;
        public static bool isInitialized;

        public virtual void OnCreate() { Console.WriteLine($"{GetType()} is Created"); }
        public virtual void Initialize() { Console.WriteLine($"{GetType()} is Initialized"); OnInitialized?.Invoke(); isInitialized = true; }

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
    }
}
