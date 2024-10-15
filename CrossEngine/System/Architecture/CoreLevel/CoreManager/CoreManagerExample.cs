namespace CrossEngine.System.Core
{
    public class CoreManagerExample : CoreManagerBase
    {
        public override void InitCoreComponentsConfigMap()
        {
            _coreConfigsMap[TypeConfigs.ConsoleCore] = new ConsoleCoreConfig();
            _coreConfigsMap[TypeConfigs.FormCore] = new FormCoreConfig();
        }
    }
}
