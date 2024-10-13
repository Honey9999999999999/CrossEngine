using CrossEngine.System.Architecture.Scene;
using System.Collections;

namespace CrossEngine.System.Arhitecture.Core
{
    public class Core : ICore
    {
        public event Action? OnInitialized;
        public bool isInitialized => _isInitialized;

        private bool _isInitialized;

        private CoreComponents _coreBase;

        private Thread update;

        public Core(CoreComponentsConfig _coreConfig)
        {
            _coreBase = new CoreComponents(_coreConfig);
            update = new(Update);
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

            OnInitialized?.Invoke();
            _isInitialized = true;
        }        

        public void RunPlayMode()
        {
            IEnumerator startPlayModeRoutine = StartPlayModeRoutine();
            while (startPlayModeRoutine.MoveNext()) ;
        }

        private IEnumerator StartPlayModeRoutine()
        {
            List<CrossBehaviour> rootGAmeObjects = SceneManagerBase.instance.GetActiveScene().GetRootGameObjects();

            foreach (var gameObject in rootGAmeObjects)
            {
                gameObject.Awake();
            }
            yield return null;

            foreach (var gameObject in rootGAmeObjects)
            {
                gameObject.Start();
            }
            yield return null;

            RunThreadUpdate();
        }

        private void RunThreadUpdate()
        {
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
            List<CrossBehaviour> rootGAmeObjects = SceneManagerBase.instance.GetActiveScene().GetRootGameObjects();

            while (true)
            {
                foreach (var gameObject in rootGAmeObjects)
                {
                    gameObject.Update();
                }
                yield return null;
            }
        }
        private IEnumerator FixedUpdateRoutine()
        {
            List<CrossBehaviour> rootGAmeObjects = SceneManagerBase.instance.GetActiveScene().GetRootGameObjects();

            while (true)
            {
                foreach (var gameObject in rootGAmeObjects)
                {
                    gameObject.FixedUpdate();
                }
                yield return new WaitForSeconds(0.02d);
            }
        }

        public void StopRunPlayMode()
        {
            IEnumerator stopPlayModeRoutine = StopPlayModeRoutine();
            while (stopPlayModeRoutine.MoveNext()) ;
        }

        private IEnumerator StopPlayModeRoutine()
        {
            //_coreBase.SendOnApplicationQuitToAllCoreComponents();
            yield return null;

            //_coreBase.SendOnDisableToAllCoreComponents();
            yield return null;

            //_coreBase.SendOnDestroyToAllCoreComponents();
            yield return null;

            StopThreadUpdate();
        }

        private void StopThreadUpdate()
        {
            update.Abort();
            update.Join();
        }
    }
}
