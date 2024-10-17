namespace CrossEngine.System.Kernel
{
    internal class ConsoleCoreConfig : CoreLoaderConfig
    {
        protected override void CreateAllCoreComponents()
        {
            CreateComponent<FileManager>();
            CreateComponent<SceneManager>();
            CreateComponent<Input>();
        }
    }
}
