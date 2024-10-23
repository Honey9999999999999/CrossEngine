using CrossEngine.Render;

namespace CrossEngine.System.Kernel
{
    internal class ConsoleCoreConfig : CoreLoaderConfig
    {
        protected override void CreateAllCoreComponents()
        {
            CreateComponent<FileManager>();
            CreateComponent<ConsoleScreen>();
            CreateComponent<SceneManager>();
            CreateComponent<SceneTree>();
            CreateComponent<Time>();
            CreateComponent<Input>();
        }
    }
}
