using CrossEngine.System.Kernel;
using System.Collections;
using System.Formats.Tar;
using System.Xml.Serialization;

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

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Scene));
        XmlSerializer xmlSerializer1 = new XmlSerializer(typeof(List<string>));

        public SceneManager()
        {
            string curFile = @"d:\Projects\CrossEngine\CrossEngine\bin\Debug\net8.0\ScenesMap.xml";
            if (File.Exists(curFile))
            {
                using FileStream fs1 = new FileStream($"{DICTIONARY_MAP_NAME}.xml", FileMode.OpenOrCreate);

                _scenesMap = instance.xmlSerializer1.Deserialize(fs1) as List<string>;
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

            using FileStream fs = new FileStream($"{name}.xml", FileMode.OpenOrCreate);
            instance.xmlSerializer.Serialize(fs, scene);

            instance._scenesMap.Add(name);

            using FileStream fs1 = new FileStream($"{DICTIONARY_MAP_NAME}.xml", FileMode.OpenOrCreate);
            instance.xmlSerializer1.Serialize(fs1, instance._scenesMap);
        }





        //internal void SetIndexes()
        //{
        //    for (int i = 0; i < _sceneConfigsMap.Values.Count; i++)
        //    {
        //        _sceneConfigsMap.Values.ToArray()[i].SetIndex(i);
        //    }
        //}



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

        private Scene GetScene(string path)
        {
            using FileStream fs = new FileStream($"{path}.xml", FileMode.OpenOrCreate);
            return instance.xmlSerializer.Deserialize(fs) as Scene;
        }



        public static void StartActiveScene()
        {
            IEnumerator loadSceneRoutine = instance.StartSceneRoutine(GetActiveScene());
            while (loadSceneRoutine.MoveNext()) ;

            OnSceneStarted?.Invoke();
        }

        private IEnumerator StartSceneRoutine(Scene scene)
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
            IEnumerator uploadSceneRoutine = StopSceneRoutine(instance._activeScene.GetRootGameObjects());
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
