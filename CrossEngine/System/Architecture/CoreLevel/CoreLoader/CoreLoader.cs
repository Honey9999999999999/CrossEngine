using System.Collections;

namespace CrossEngine.System.Kernel
{
    internal sealed class CoreLoader : Singleton<CoreLoader>
    {
        public static void LoadCore(CoreLoaderConfig config)
        {
            new Core();
            IEnumerator loadCoreRoutine = instance.LoadCoreRoutine(config.coreComponents);
            while (loadCoreRoutine.MoveNext()) ;
        }

        private IEnumerator LoadCoreRoutine(List<IInitializeble> coreComponents)
        {
            SendOnCreateToAllCoreComponents(coreComponents);
            yield return null;

            Console.WriteLine();

            SendInitializeToAllCoreComponents(coreComponents);
            yield return null;
        }

        private void SendOnCreateToAllCoreComponents(List<IInitializeble> coreComponents)
        {
            foreach (var _coreComponent in coreComponents)
            {
                _coreComponent.OnCreate();
            }
        }

        private void SendInitializeToAllCoreComponents(List<IInitializeble> coreComponents)
        {
            foreach (var _coreComponent in coreComponents)
            {
                _coreComponent.Initialize();
            }
        }
    }
}
