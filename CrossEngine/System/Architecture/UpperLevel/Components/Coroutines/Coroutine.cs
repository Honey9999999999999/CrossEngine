using System.Collections;

namespace CrossEngine
{
    public sealed class Coroutine
    {
        internal IEnumerator _routine;

        private Coroutine(IEnumerator routine)
        {
            _routine = routine;
        }

        internal static Coroutine CreateCoroutine(IEnumerator routine)
        {
            return new(routine);
        }
    }
}
