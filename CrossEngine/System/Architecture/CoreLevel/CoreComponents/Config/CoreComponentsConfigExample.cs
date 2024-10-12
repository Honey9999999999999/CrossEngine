using CrossEngine.System.Arhitecture.UpperLevel.Components.Coroutines;
using CrossEngine.System.Interfaces;

namespace CrossEngine.System.Arhitecture.BaseLevel.CoreBase.Config
{
    public class CoreComponentsConfigExample : CoreComponentsConfig
    {
        public override Dictionary<Type, ICrossBehaviour> CreateAllCoreComponents()
        {
            _coreComponents = new();

            CreateComponent<CoroutinesBase>();

            return _coreComponents;
        }
    }
}
