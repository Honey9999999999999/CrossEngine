using System.Collections;

namespace CrossEngine.System.Kernel
{
    internal sealed class CoreLoader : Singleton<CoreLoader>
    {
        public static void LoadCore(CoreLoaderConfig config)
        {
            _ = new Core();
            IEnumerator loadCoreRoutine = Instance.LoadCoreRoutine(config.CoreComponents);
            while (loadCoreRoutine.MoveNext()) ;
        }

        private IEnumerator LoadCoreRoutine(Dictionary<Type, IInitializeble> coreComponents)
        {
            SendOnCreateToAllCoreComponents(coreComponents);
            yield return null;

            Console.WriteLine();

            SendInitializeToAllCoreComponents(coreComponents);
            yield return null;
        }

        private void SendOnCreateToAllCoreComponents(Dictionary<Type, IInitializeble> coreComponents)
        {
            foreach (var _coreComponent in coreComponents.Values)
            {
                _coreComponent.OnCreate();
            }
        }

        private void SendInitializeToAllCoreComponents(Dictionary<Type, IInitializeble> coreComponents)
        {
            foreach (var _coreComponent in coreComponents.Values)
            {
                _coreComponent.Initialize();
            }
        }
    }
}
