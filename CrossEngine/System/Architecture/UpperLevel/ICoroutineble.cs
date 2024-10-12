using CrossEngine.System.Arhitecture.UpperLevel.Components.Coroutines;
using System.Collections;

namespace CrossEngine.System.Arhitecture.UpperLevel
{
    public interface ICoroutineble
    {
        public Coroutine StartCoroutine(IEnumerator routine);
        public void StopCoroutine(Coroutine coroutine);
    }
}
