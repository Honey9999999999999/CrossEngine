using CrossEngine.System.Arhitecture.BaseLevel.CoreBase.Config;

namespace CrossEngine.System.Arhitecture.CoreLevel.CoreManager
{
    public class CoreManagerExample : CoreManagerBase
    {
        public override void InitCoreComponentsConfigMap()
        {
            _coreConfigsMap[TypeConfigs.Base] = new CoreComponentsConfigExample();
        }
    }
}
