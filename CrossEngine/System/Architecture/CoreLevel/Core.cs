using System.Collections;

namespace CrossEngine.System.Kernel
{
    internal class Core : Singleton<Core>
    {
        public static event Action? OnInitialized;
        public static event Action? OnUpdateStarted;
        public static event Action? OnPreUpdate;
        public static event Action? OnLateUpdate;
        public static event Action? OnUpdateStoped;

        public bool isInitialized => _isInitialized;
        public bool isRunPlayMode => _isRunPlayMode;

        private bool _isRunPlayMode;
        private bool _isInitialized;

        private Thread update;

        public Core()
        {
            update = new(Update);
        }        

        public static void RunPlayMode()
        {
            instance.RunThreadUpdate();
            instance._isRunPlayMode = true;
        }

        private void RunThreadUpdate()
        {
            update.Start();            
        }

        public static void CoreRequiest(Action action)
        {
            if (instance._isRunPlayMode)
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

        public static void StopRunPlayMode()
        {
            instance._isRunPlayMode = false;
        }
    }
}
