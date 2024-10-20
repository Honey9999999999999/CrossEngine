using System.Collections;

namespace CrossEngine.System
{
    public interface ICoroutineble
    {
        public Coroutine StartCoroutine(IEnumerator routine);
        public void StopCoroutine(Coroutine coroutine);
    }
}
