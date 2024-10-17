using System.Reflection;
using System.Xml.Serialization;

namespace CrossEngine.System
{
    internal class FileManager : CoreComponent<FileManager>
    {
        private const string NAME_MAIN_DIRECTORY = "Resources";

        private readonly string _pathToEngineDirectory;
        private Dictionary<SavePlace, string> _pathMap;

        public FileManager()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            _pathToEngineDirectory = Path.GetDirectoryName(location) ?? throw new CrossException("Path to direction engine is not find.");

            _pathMap = new()
            {
                [SavePlace.Scenes] = $"{_pathToEngineDirectory}\\{NAME_MAIN_DIRECTORY}\\{SavePlace.Scenes}"
            };


            Directory.CreateDirectory($"{_pathToEngineDirectory}\\{NAME_MAIN_DIRECTORY}");

            foreach (var path in _pathMap.Values.ToArray())
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void SaveInXml<T>(T obj, string name, SavePlace location) where T : class
        {
            using FileStream fs = new($"{instance._pathMap[location]}\\{name}.xml", FileMode.OpenOrCreate);
            new XmlSerializer(typeof(T)).Serialize(fs, obj);
        }

        public static T LoadFromXml<T>(SavePlace location, string name) where T : class
        {
            if (!IsPathExist(location, name))
            {
                throw new CrossException($"This path : '{instance._pathMap[location]}\\{name}.xml' has not exist.");
            }

            using FileStream fs = new FileStream($"{instance._pathMap[location]}\\{name}.xml", FileMode.OpenOrCreate);

            return new XmlSerializer(typeof(T)).Deserialize(fs) as T;
        }

        public static bool IsPathExist(SavePlace location, string name)
        {
            if (File.Exists($"{instance._pathMap[location]}\\{name}.xml"))
            {
                return true;
            }

            return false;
        }
    }
}
