using CrossEngine.System.Kernel;
using System.Collections;
using System.Formats.Tar;
using System.Xml.Serialization;
using static System.Formats.Asn1.AsnWriter;

namespace CrossEngine.System
{
    public sealed class SceneManager : CoreComponent<SceneManager>
    {
        public static event Action? OnSceneStarted;
        public static event Action? OnSceneStoped;

        public static event Action? OnSceneLoaded;

        public int sceneCount => _scenesMap.Count;

        private List<string> _scenesMap;

        private Scene? _activeScene;

        private const string DICTIONARY_MAP_NAME = "ScenesMap";

        public SceneManager()
        {
            if (FileManager.IsPathExist(SavePlace.Scenes, DICTIONARY_MAP_NAME))
            {
                _scenesMap = FileManager.LoadFromXml<List<string>>(SavePlace.Scenes, DICTIONARY_MAP_NAME);
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



        public static void CreateScene(string name)
        {
            Scene scene = new()
            {
                name = name,
                index = instance._scenesMap.Count
            };

            FileManager.SaveInXml(scene, name, SavePlace.Scenes);

            instance._scenesMap.Add(name);

            FileManager.SaveInXml(instance._scenesMap, DICTIONARY_MAP_NAME, SavePlace.Scenes);
        }



        public static Scene GetActiveScene()
        {
            if (instance._activeScene == null)
            {
                throw new CrossException("Scene is not loaded!");
            }

            return instance._activeScene;
        }

        public static Scene GetSceneAt(int index)
        {
            return instance.GetScene(instance._scenesMap[index]);
        }
        public static Scene GetSceneByName(string name)
        {
            return instance.GetScene(name);
        }
        


        public static void LoadScene(int index)
        {
            LoadScene(instance._scenesMap[index]);
        }
        public static void LoadScene(string name)
        {
            instance._activeScene = instance.GetScene(name);

            OnSceneLoaded?.Invoke();
        }

        private Scene GetScene(string name)
        {
            return FileManager.LoadFromXml<Scene>(SavePlace.Scenes, name);
        }



        public static void StartActiveScene()
        {
            IEnumerator loadSceneRoutine = instance.StartSceneRoutine(GetActiveScene());
            while (loadSceneRoutine.MoveNext()) ;

            OnSceneStarted?.Invoke();
        }

        private IEnumerator StartSceneRoutine(Scene scene)
        {
            GameObject[] rootGameObjects = [.. scene.GetRootGameObjects()];

            foreach (var gameObject in rootGameObjects)
            {
                foreach (var commponent in gameObject.GetComponents<CrossBehaviour>())
                {
                    if (commponent.Enabled)
                    {
                        commponent.Awake();
                    }
                }                
            }
            yield return null;

            foreach (var gameObject in rootGameObjects)
            {
                foreach (var commponent in gameObject.GetComponents<CrossBehaviour>())
                {
                    if (commponent.Enabled)
                    {
                        commponent.OnEnable();
                    }
                }
            }
            yield return null;

            foreach (var gameObject in rootGameObjects)
            {
                foreach (var commponent in gameObject.GetComponents<CrossBehaviour>())
                {
                    if (commponent.Enabled)
                    {
                        commponent.Start();
                    }
                }
            }
        }

        private static void StopActiveScene()
        {
            IEnumerator uploadSceneRoutine = StopSceneRoutine(instance._activeScene.GetRootGameObjects());
            while (uploadSceneRoutine.MoveNext()) ;

            OnSceneStoped?.Invoke();
        }

        private static IEnumerator StopSceneRoutine(List<GameObject> rootGameObjects)
        {
            foreach (var gameObject in rootGameObjects)
            {
                foreach (var commponent in gameObject.GetComponents<CrossBehaviour>())
                {
                    commponent.OnApplicationQuit();
                }
            }
            yield return null;

            foreach (var gameObject in rootGameObjects)
            {
                foreach (var commponent in gameObject.GetComponents<CrossBehaviour>())
                {
                    commponent.OnDisable();
                }
            }
            yield return null;

            foreach (var gameObject in rootGameObjects)
            {
                foreach (var commponent in gameObject.GetComponents<CrossBehaviour>())
                {
                    commponent.OnDestroy();
                }
            }
        }
    }
}
