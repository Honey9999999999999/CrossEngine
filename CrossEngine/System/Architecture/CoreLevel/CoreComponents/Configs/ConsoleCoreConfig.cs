namespace CrossEngine.System.Core
{
    public class ConsoleCoreConfig : CoreComponentsConfig
    {
        public override Dictionary<Type, IInitializeble> CreateAllCoreComponents()
        {
            _coreComponents = new();

            CreateComponent<SceneManager>();
            CreateComponent<Input>();

            return _coreComponents;
        }
    }
}
