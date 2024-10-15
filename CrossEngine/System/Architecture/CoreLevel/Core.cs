using System.Collections;

namespace CrossEngine.System.Core
{
    internal class Core : ICore
    {
        public event Action? OnInitialized;
        public event Action? OnUpdateStarted;
        public event Action? OnPreUpdate;
        public event Action? OnLateUpdate;
        public event Action? OnUpdateStoped;

        public bool isInitialized => _isInitialized;
        public bool isRunPlayMode => _isRunPlayMode;

        private bool _isRunPlayMode;
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
            RunThreadUpdate();
            _isRunPlayMode = true;
        }

        private void RunThreadUpdate()
        {
            update.Start();
        }

        public void CoreRequiest(Action action)
        {
            if (_isRunPlayMode)
            {
                void Task()
                {
                    action.Invoke();
                    OnPreUpdate -= Task;
                }

                OnPreUpdate += Task;
            }
            else
            {
                action.Invoke();
            }
        }

        private void Update()
        {
            IEnumerator updateRoutine = UpdateRoutine();
            IEnumerator fixedUpdateRoutine = FixedUpdateRoutine();

            OnUpdateStarted?.Invoke();

            while (_isRunPlayMode)
            {
                OnPreUpdate?.Invoke();

                if (fixedUpdateRoutine.Current is ICoroutineDelay delay && !delay.Ready) continue;
                fixedUpdateRoutine.MoveNext();

                updateRoutine.MoveNext();

                OnLateUpdate?.Invoke();
            }

            OnUpdateStoped?.Invoke();
        }

        private IEnumerator UpdateRoutine()
        {
            List<CrossBehaviour> rootGameObjects = SceneManagerBase.GetActiveScene().GetRootGameObjects();

            while (true)
            {
                foreach (var gameObject in rootGameObjects)
                {
                    gameObject.Update();
                }
                yield return null;
            }
        }
        private IEnumerator FixedUpdateRoutine()
        {
            List<CrossBehaviour> rootGAmeObjects = SceneManagerBase.GetActiveScene().GetRootGameObjects();

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
            _isRunPlayMode = false;
        }
    }
}
