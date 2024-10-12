using CrossEngine.System.Interfaces;
using System.Collections;

namespace CrossEngine.System.Arhitecture.SceneLevel
{
    public class Core : ICore
    {
        public event Action OnInitialized;
        public bool isInitialized { get; private set; }

        private CoreComponents _coreBase;

        public Core(CoreComponentsConfig _coreConfig)
        {
            _coreBase = new CoreComponents(_coreConfig);
        }

        public void InitializeCore()
        {
            IEnumerator routine = InitializeCoreRoutine();

            while (routine.MoveNext()) ;
        }

        private IEnumerator InitializeCoreRoutine()
        {
            _coreBase.SendOnCreateToAllCoreComponents();
            yield return null;

            _coreBase.SendInitializeToAllCoreComponents();
            yield return null;

            _coreBase.SendAwakeToAllCoreComponents();
            yield return null;

            _coreBase.SendStartToAllCoreComponents();
            yield return null;

            OnInitialized?.Invoke();
            isInitialized = true;
        }

        public void Run()
        {
            Thread update = new(Update);
            update.Start();
        }

        private void Update()
        {
            IEnumerator updateRoutine = UpdateRoutine();
            IEnumerator fixedUpdateRoutine = FixedUpdateRoutine();

            while (true)
            {
                updateRoutine.MoveNext();

                if (fixedUpdateRoutine.Current is WaitForSeconds wait && wait.time > DateTime.UtcNow) continue;

                fixedUpdateRoutine.MoveNext();
            }
        }

        private IEnumerator UpdateRoutine()
        {
            while (true)
            {
                _coreBase.SendUpdateToAllCoreComponents();
                yield return null;
            }
        }
        private IEnumerator FixedUpdateRoutine()
        {
            while (true)
            {
                _coreBase.SendFixedUpdateToAllCoreComponents();
                yield return new WaitForSeconds(0.02d);
            }
        }
    }
}
