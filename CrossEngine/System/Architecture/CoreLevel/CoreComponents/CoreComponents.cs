namespace CrossEngine.System.Arhitecture.Core
{
    public class CoreComponents : ICoreComponents
    {
        private CoreComponentsConfig _config;

        private Dictionary<Type, IInitializeble> _coreComponents;

        public CoreComponents(CoreComponentsConfig config)
        {
            _config = config;
            _coreComponents = _config.CreateAllCoreComponents();
        }

        public void SendOnCreateToAllCoreComponents()
        {
            foreach (var _coreComponent in _coreComponents.Values)
            {
                _coreComponent.OnCreate();
            }
        }

        public void SendInitializeToAllCoreComponents()
        {
            foreach (var _coreComponent in _coreComponents.Values)
            {
                _coreComponent.Initialize();
            }
        }

        //public void SendAwakeToAllCoreComponents()
        //{
        //    foreach (var _coreComponent in _coreComponents.Values)
        //    {
        //        _coreComponent.Awake();
        //    }
        //}

        //public void SendStartToAllCoreComponents()
        //{
        //    foreach (var _coreComponent in _coreComponents.Values)
        //    {
        //        _coreComponent.Start();
        //    }
        //}

        //public void SendUpdateToAllCoreComponents()
        //{
        //    foreach (var _coreComponent in _coreComponents.Values)
        //    {
        //        _coreComponent.Update();
        //    }
        //}

        //public void SendFixedUpdateToAllCoreComponents()
        //{
        //    foreach (var _coreComponent in _coreComponents.Values)
        //    {
        //        _coreComponent.FixedUpdate();
        //    }
        //}

        public TCoreComponent GetComponent<TCoreComponent>() where TCoreComponent : IInitializeble
        {
            return (TCoreComponent)_coreComponents[typeof(TCoreComponent)];
        }
    }
}
