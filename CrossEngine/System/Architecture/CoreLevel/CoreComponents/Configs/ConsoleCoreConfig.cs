using CrossEngine.System.Architecture.Scene;

namespace CrossEngine.System.Arhitecture.Core
{
    public class ConsoleCoreConfig : CoreComponentsConfig
    {
        public override Dictionary<Type, IInitializeble> CreateAllCoreComponents()
        {
            _coreComponents = new();

            CreateComponent<SceneManager>();

            return _coreComponents;
        }
    }
}
