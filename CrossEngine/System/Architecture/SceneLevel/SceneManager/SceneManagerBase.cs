using System.Collections;

namespace CrossEngine.System.Architecture.Scene
{
    internal abstract class SceneManagerBase : Initializeble, ISceneManager
    {
        public event Action? OnSceneLoaded;
        public event Action? OnSceneUpLoaded;

        public static SceneManagerBase? instance;

        public int sceneCount => _sceneConfigsMap.Values.Count;

        private Dictionary<string, SceneBase> _sceneConfigsMap;

        private IScene? _activeScene;

        public SceneManagerBase()
        {
            if (instance != null)
            {
                throw new CrossException("Scene manager already was created.");
            }

            _sceneConfigsMap = [];
            instance = this;
        }

        public override void OnCreate()
        {
            
        }

        public void CreateScene(string name)
        {
            _sceneConfigsMap[name] = new Scene(name);
        }

        internal void SetIndexes()
        {
            for (int i = 0; i < _sceneConfigsMap.Values.Count; i++)
            {
                _sceneConfigsMap.Values.ToArray()[i].SetIndex(i);
            }
        }

        public IScene GetActiveScene()
        {
            if (_activeScene == null)
            {
                throw new CrossException("Scene is not loaded!");
            }

            return _activeScene;
        }

        public IScene GetSceneAt(int index)
        {
            return _sceneConfigsMap.Values.ToArray()[index];
        }

        public IScene GetSceneByName(string name)
        {
            return _sceneConfigsMap[name];
        }

        public void LoadScene(int index)
        {
            LoadScene(GetSceneAt(index));
        }

        public void LoadScene(string name)
        {
            LoadScene(GetSceneByName(name));
        }

        private void LoadScene(IScene scene)
        {
            UploadScene();

            IEnumerator loadSceneRoutine = LoadSceneRoutine(scene);
            while (loadSceneRoutine.MoveNext()) ;

            _activeScene = scene;

            OnSceneLoaded?.Invoke();
        }

        private IEnumerator LoadSceneRoutine(IScene scene)
        {
            List<CrossBehaviour> rootGAmeObjects = scene.GetRootGameObjects();

            foreach (var gameObject in rootGAmeObjects)
            {
                gameObject.Awake();
            }
            yield return null;

            foreach (var gameObject in rootGAmeObjects)
            {
                gameObject.OnEnable();
            }
            yield return null;

            foreach (var gameObject in rootGAmeObjects)
            {
                gameObject.Start();
            }
            yield return null;
        }

        private void UploadScene()
        {
            if (_activeScene != null)
            {
                IEnumerator uploadSceneRoutine = UploadSceneRoutine(_activeScene.GetRootGameObjects());
                while (uploadSceneRoutine.MoveNext()) ;

                OnSceneUpLoaded?.Invoke();
            }
        }
        private IEnumerator UploadSceneRoutine(List<CrossBehaviour> rootGAmeObjects)
        {
            foreach (var gameObject in rootGAmeObjects)
            {
                gameObject.OnApplicationQuit();
            }
            yield return null;

            foreach (var gameObject in rootGAmeObjects)
            {
                gameObject.OnDisable();
            }
            yield return null;

            foreach (var gameObject in rootGAmeObjects)
            {
                gameObject.OnDestroy();
            }
            yield return null;
        }
    }
}
