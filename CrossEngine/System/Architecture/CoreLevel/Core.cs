using CrossEngine.System.Architecture.Scene;
using System.Collections;

namespace CrossEngine.System.Arhitecture.Core
{
    internal class Core : ICore
    {
        public event Action? OnInitialized;
        private event Action? OnPreUpdate;

        public bool isInitialized => _isInitialized;

        public bool isRunPlayMode => _isRunPlayMode;
        private bool _isRunPlayMode;

        private bool _isInitialized;

        private CoreComponents _coreBase;

        private Thread update;

        private TaskManager _taskManager;

        public Core(CoreComponentsConfig _coreConfig)
        {
            _coreBase = new CoreComponents(_coreConfig);
            update = new(Update);
            _taskManager = new();
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
            _isRunPlayMode = true;
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

        internal void AddTask(Action action)
        {
            if (_isRunPlayMode)
            {
                void Add()
                {
                    _taskManager.AddTask(new Task(action));
                    OnPreUpdate -= Add;
                }

                OnPreUpdate += Add;
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

            while (true)
            {
                OnPreUpdate?.Invoke();
                _taskManager.RunTasks();

                if (fixedUpdateRoutine.Current is ICoroutineDelay delay && !delay.Ready) continue;

                fixedUpdateRoutine.MoveNext();

                updateRoutine.MoveNext();
            }
        }

        private IEnumerator UpdateRoutine()
        {
            List<CrossBehaviour> rootGameObjects = SceneManagerBase.instance.GetActiveScene().GetRootGameObjects();

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
            _isRunPlayMode = false;
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
