using CrossEngine.System.Kernel;
using System.Collections;

namespace CrossEngine.System
{
    public sealed class SceneManager : CoreComponent<SceneManager>
    {
        public static event Action? OnSceneStarted;
        public static event Action? OnSceneStoped;

        public static event Action? OnSceneUploading;
        public static event Action? OnSceneLoaded;

        public int sceneCount => _scenesMap.Count;

        private List<string> _scenesMap;

        private Scene? _activeScene;

        private const string DICTIONARY_MAP_NAME = "ScenesMap";

        private static SceneManager Instance => Engine.GetCoreComponent<SceneManager>();
        private FileManager _fileManager;

        public SceneManager()
        {
            _fileManager = Engine.GetCoreComponent<FileManager>();

            if (_fileManager.IsPathExist(SavePlace.Scenes, DICTIONARY_MAP_NAME))
            {
                _scenesMap = _fileManager.LoadFromXml<List<string>>(SavePlace.Scenes, DICTIONARY_MAP_NAME);
            }
            else
            {
                _scenesMap = [];
            }
        }

        public override void OnCreate()
        {
            Core.OnUpdateStarted += StartActiveScene;
            Core.OnUpdateStoped += StopActiveScene;

            base.OnCreate();
        }

        public override void Initialize()
        {
            if (_scenesMap.Count == 0)
            {
                CreateScene("Default Scene");
                CrossMessager.PrintWarningMessage("Emergency create scene with name 'Default Scene' is Done.");
            }

            LoadScene(0);

            base.Initialize();
        }



        public void CreateScene(string name)
        {
            Scene scene = new()
            {
                Name = name,
                Index = _scenesMap.Count
            };

            _fileManager.SaveInXml(scene, name, SavePlace.Scenes);

            _scenesMap.Add(scene.Name);

            _fileManager.SaveInXml(_scenesMap, DICTIONARY_MAP_NAME, SavePlace.Scenes);

            _activeScene = scene;
        }



        public static Scene GetActiveScene()
        {
            if (Instance._activeScene == null)
            {
                throw new CrossException("Scene is not loaded!");
            }

            return Instance._activeScene;
        }
        internal static bool TryGetActiveScene(out Scene? scene)
        {
            bool isContains = Instance._activeScene != null;
            scene = isContains ? Instance._activeScene : null;
            return isContains;
        }

        public static Scene GetSceneAt(int index)
        {
            return Instance.GetScene(Instance._scenesMap[index]);
        }
        public static Scene GetSceneByName(string name)
        {
            return Instance.GetScene(name);
        }



        public static void LoadScene(int index)
        {
            LoadScene(Instance._scenesMap[index]);
        }
        public static void LoadScene(string name)
        {
            if (Instance._activeScene != null)
            {
                OnSceneUploading?.Invoke();
            }

            Instance._activeScene = Instance.GetScene(name);

            OnSceneLoaded?.Invoke();
        }

        private Scene GetScene(string name)
        {
            return _fileManager.LoadFromXml<Scene>(SavePlace.Scenes, name);
        }



        public void StartActiveScene()
        {
            IEnumerator loadSceneRoutine = StartSceneRoutine(GetActiveScene());
            while (loadSceneRoutine.MoveNext()) ;

            OnSceneStarted?.Invoke();
        }

        private IEnumerator StartSceneRoutine(Scene scene)
        {
            Transform[] rootGameObjects = [.. scene.RootNode.Transform.GetChilds()];

            foreach (var transform in rootGameObjects)
            {
                foreach (var commponent in transform.GetComponents<CrossBehaviour>())
                {
                    if (commponent.Enabled)
                    {
                        commponent.Awake();
                    }
                }
            }
            yield return null;

            foreach (var transform in rootGameObjects)
            {
                foreach (var commponent in transform.GetComponents<CrossBehaviour>())
                {
                    if (commponent.Enabled)
                    {
                        commponent.OnEnable();
                    }
                }
            }
            yield return null;

            foreach (var transform in rootGameObjects)
            {
                foreach (var commponent in transform.GetComponents<CrossBehaviour>())
                {
                    if (commponent.Enabled)
                    {
                        commponent.Start();
                    }
                }
            }
        }

        private void StopActiveScene()
        {
            IEnumerator uploadSceneRoutine = StopSceneRoutine(_activeScene.RootNode.Transform.GetChilds());
            while (uploadSceneRoutine.MoveNext()) ;

            OnSceneStoped?.Invoke();
        }

        private static IEnumerator StopSceneRoutine(Transform[] rootGameObjects)
        {
            foreach (var transform in rootGameObjects)
            {
                foreach (var commponent in transform.GetComponents<CrossBehaviour>())
                {
                    commponent.OnApplicationQuit();
                }
            }
            yield return null;

            foreach (var transform in rootGameObjects)
            {
                foreach (var commponent in transform.GetComponents<CrossBehaviour>())
                {
                    commponent.OnDisable();
                }
            }
            yield return null;

            foreach (var transform in rootGameObjects)
            {
                foreach (var commponent in transform.GetComponents<CrossBehaviour>())
                {
                    commponent.OnDestroy();
                }
            }
        }
    }
}
