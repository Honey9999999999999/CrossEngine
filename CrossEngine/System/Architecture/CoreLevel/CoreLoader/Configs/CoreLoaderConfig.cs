namespace CrossEngine.System.Kernel
{
    internal abstract class CoreLoaderConfig
    {
        public Dictionary<Type, IInitializeble> CoreComponents
        {
            get
            {
                if(_coreComponents.Count == 0)
                    CreateAllCoreComponents();

                return _coreComponents;
            }
        }

        private readonly Dictionary<Type, IInitializeble> _coreComponents = [];


        protected abstract void CreateAllCoreComponents();

        protected void CreateComponent<TCoreComponent>() where TCoreComponent : IInitializeble, new()
        {
            _coreComponents[typeof(TCoreComponent)] = new TCoreComponent();
        }

        public TCoreComponent GetCoreComponent<TCoreComponent>() where TCoreComponent : IInitializeble, new()
        {
            return (TCoreComponent)CoreComponents[typeof(TCoreComponent)];
        }
    }
}
