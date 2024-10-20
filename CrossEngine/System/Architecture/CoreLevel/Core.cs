using System.Collections;
using CrossEngine;

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
        public static bool isRunPlayMode => _isRunPlayMode;

        private static bool _isRunPlayMode;
        private bool _isInitialized;

        private readonly Thread update;

        public Core()
        {
            update = new(Update);
        }        

        public static void RunPlayMode()
        {
            _isRunPlayMode = true;
            instance.RunThreadUpdate();            
        }

        private void RunThreadUpdate()
        {
            update.Start();            
        }

        public static void CoreRequiest(Action action)
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

                if (fixedUpdateRoutine.Current is not ICoroutineDelay delay || delay.Ready)
                    fixedUpdateRoutine.MoveNext();

                updateRoutine.MoveNext();

                OnLateUpdate?.Invoke();
            }

            OnUpdateStoped?.Invoke();
        }

        private IEnumerator UpdateRoutine()
        {
            while (true)
            {
                Transform[] rootObjects = SceneManager.GetActiveScene().RootNode.Transform.GetChilds();

                foreach (var transform in rootObjects)
                {
                    foreach (CrossBehaviour component in transform.GetComponents<CrossBehaviour>())
                    {
                        component.Update();
                    }
                }
                yield return null;
            }
        }
        private IEnumerator FixedUpdateRoutine()
        {
            while (true)
            {
                Transform[] rootObjects = SceneManager.GetActiveScene().RootNode.Transform.GetChilds();

                foreach (var transform in rootObjects)
                {
                    foreach(CrossBehaviour component in transform.GetComponents<CrossBehaviour>())
                    {
                        component.FixedUpdate();
                    }
                }
                yield return new WaitForSeconds(0.02d);
            }
        }

        public static void StopRunPlayMode()
        {
            _isRunPlayMode = false;
        }
    }
}
