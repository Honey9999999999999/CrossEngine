using CrossEngine.System.Kernel;
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
                [SavePlace.Scenes] = $"{_pathToEngineDirectory}\\{NAME_MAIN_DIRECTORY}\\{SavePlace.Scenes}",
                [SavePlace.Screen] = $"{_pathToEngineDirectory}\\{NAME_MAIN_DIRECTORY}\\{SavePlace.Screen}"
            };


            Directory.CreateDirectory($"{_pathToEngineDirectory}\\{NAME_MAIN_DIRECTORY}");

            foreach (var path in _pathMap.Values.ToArray())
            {
                Directory.CreateDirectory(path);
            }
        }

        public void SaveInXml<T>(T obj, string name, SavePlace location) where T : class
        {
            using FileStream fs = new($"{_pathMap[location]}\\{name}.xml", FileMode.OpenOrCreate);
            new XmlSerializer(typeof(T)).Serialize(fs, obj);
        }

        public T LoadFromXml<T>(SavePlace location, string name) where T : class
        {
            if (!IsPathExist(location, name))
            {
                throw new CrossException($"This path : '{_pathMap[location]}\\{name}.xml' has not exist.");
            }

            using FileStream fs = new FileStream($"{_pathMap[location]}\\{name}.xml", FileMode.OpenOrCreate);

            return new XmlSerializer(typeof(T)).Deserialize(fs) as T;
        }

        public bool IsPathExist(SavePlace location, string name)
        {
            return File.Exists($"{_pathMap[location]}\\{name}.xml");
        }
    }
}
