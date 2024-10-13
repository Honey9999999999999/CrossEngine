namespace CrossEngine.System.Arhitecture.Core
{
    public class FormCoreConfig : CoreComponentsConfig
    {
        public override Dictionary<Type, IInitializeble> CreateAllCoreComponents()
        {
            _coreComponents = new();

            CreateComponent<CoroutinesBase>();

            return _coreComponents;
        }
    }
}
