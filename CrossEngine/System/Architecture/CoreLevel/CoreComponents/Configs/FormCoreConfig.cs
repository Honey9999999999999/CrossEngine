namespace CrossEngine.System.Core
{
    public class FormCoreConfig : CoreComponentsConfig
    {
        public override Dictionary<Type, IInitializeble> CreateAllCoreComponents()
        {
            _coreComponents = new();


            return _coreComponents;
        }
    }
}
