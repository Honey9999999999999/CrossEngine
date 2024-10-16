namespace CrossEngine.System.Kernel
{
    internal class ConsoleCoreConfig : CoreLoaderConfig
    {
        protected override void CreateAllCoreComponents()
        {
            CreateComponent<SceneManager>();
            CreateComponent<Input>();
        }
    }
}
