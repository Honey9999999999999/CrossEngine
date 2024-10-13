using System.Collections;

namespace CrossEngine
{
    public class CoroutinesBase : CrossBehaviour
    {
        internal static CoroutinesBase instance;

        private List<IEnumerator> routines;

        public CoroutinesBase()
        {
            instance ??= this;
        }

        public override void Initialize()
        {
            routines = [];

            base.Initialize();
        }

        internal void StartRoutine(IEnumerator routine)
        {
            routines.Add(routine);
        }
        internal void StopRoutine(Coroutine coroutine)
        {
            routines.Remove(coroutine._routine);
        }

        public override void Update()
        {
            foreach (var routine in routines)
            {
                if (routine.Current is ICoroutineDelay delay && !delay.Ready) continue;

                routine.MoveNext();
            }
        }
    }
}
