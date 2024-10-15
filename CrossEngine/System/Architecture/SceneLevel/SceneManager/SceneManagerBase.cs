using System.Collections;

namespace CrossEngine.System
{
    public abstract class SceneManagerBase : CoreComponent<SceneManagerBase>, ISceneManager
    {
        public static event Action? OnSceneStarted;
        public static event Action? OnSceneStoped;

        public static event Action? OnSceneLoaded;

        public int sceneCount => _sceneConfigsMap.Values.Count;

        private Dictionary<string, SceneBase> _sceneConfigsMap;

        private SceneBase? _activeScene;

        protected SceneManagerBase()
        {
            _sceneConfigsMap = [];
        }

        public override void OnCreate()
        {
            Engine.GetCore().OnUpdateStarted += StartActiveScene;
            Engine.GetCore().OnUpdateStoped += StopActiveScene;

            base.OnCreate();
        }

        public override void Initialize()
        {
            if(_sceneConfigsMap.Count < 1)
            {
                CreateScene("Default Scene");
                CrossMessager.PrintWarningMessage("Emergency create scene with name 'Default Scene' is Done.");
            }

            _activeScene = _sceneConfigsMap.Values.ToArray()[0];

            base.Initialize();
        }

        public static void CreateScene(string name)
        {
            _instance._sceneConfigsMap[name] = new Scene(name);
        }



        internal void SetIndexes()
        {
            for (int i = 0; i < _sceneConfigsMap.Values.Count; i++)
            {
                _sceneConfigsMap.Values.ToArray()[i].SetIndex(i);
            }
        }



        public static SceneBase GetActiveScene()
        {
            if (_instance._activeScene == null)
            {
                throw new CrossException("Scene is not loaded!");
            }

            return _instance._activeScene;
        }



        public static SceneBase GetSceneAt(int index)
        {
            return _instance._sceneConfigsMap.Values.ToArray()[index];
        }
        public static SceneBase GetSceneByName(string name)
        {
            return _instance._sceneConfigsMap[name];
        }



        public static void LoadScene(int index)
        {
            LoadScene(GetSceneAt(index));
        }
        public static void LoadScene(string name)
        {
            LoadScene(GetSceneByName(name));
        }
        public static void LoadScene(SceneBase scene)
        {
            _instance._activeScene = scene;

            OnSceneLoaded?.Invoke();
        }



        public static void StartActiveScene()
        {
            IEnumerator loadSceneRoutine = _instance.StartSceneRoutine(GetActiveScene());
            while (loadSceneRoutine.MoveNext()) ;

            OnSceneStarted?.Invoke();
        }

        private IEnumerator StartSceneRoutine(IScene scene)
        {
            List<CrossBehaviour> rootGameObjects = scene.GetRootGameObjects();

            foreach (var gameObject in rootGameObjects)
            {
                gameObject.Awake();
            }
            yield return null;

            foreach (var gameObject in rootGameObjects)
            {
                gameObject.OnEnable();
            }
            yield return null;

            foreach (var gameObject in rootGameObjects)
            {
                gameObject.Start();
            }
            yield return null;
        }

        private static void StopActiveScene()
        {
            IEnumerator uploadSceneRoutine = StopSceneRoutine(_instance._activeScene.GetRootGameObjects());
            while (uploadSceneRoutine.MoveNext()) ;

            OnSceneStoped?.Invoke();
        }

        private static IEnumerator StopSceneRoutine(List<CrossBehaviour> rootGameObjects)
        {
            foreach (var gameObject in rootGameObjects)
            {
                gameObject.OnApplicationQuit();
            }
            yield return null;

            foreach (var gameObject in rootGameObjects)
            {
                gameObject.OnDisable();
            }
            yield return null;

            foreach (var gameObject in rootGameObjects)
            {
                gameObject.OnDestroy();
            }
            yield return null;
        }
    }
}
