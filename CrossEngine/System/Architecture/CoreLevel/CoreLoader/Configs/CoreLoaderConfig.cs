namespace CrossEngine.System.Kernel
{
    internal abstract class CoreLoaderConfig
    {
        public List<IInitializeble> coreComponents
        {
            get
            {
                if (_coreComponents == null)
                {
                    _coreComponents = [];
                    CreateAllCoreComponents();
                }

                return _coreComponents;
            }
        }

        private List<IInitializeble>? _coreComponents;


        protected abstract void CreateAllCoreComponents();

        protected void CreateComponent<TCrossBehaviour>() where TCrossBehaviour : IInitializeble, new()
        {
            if (_coreComponents == null)
            {
                throw new CrossException("Dictionary of components is null!!!");
            }

            _coreComponents.Add(new TCrossBehaviour());
        }
    }
}
