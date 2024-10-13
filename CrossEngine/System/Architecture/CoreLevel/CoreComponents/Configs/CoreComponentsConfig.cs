namespace CrossEngine.System.Arhitecture.Core
{
    public abstract class CoreComponentsConfig
    {
        protected Dictionary<Type, IInitializeble> _coreComponents;

        public abstract Dictionary<Type, IInitializeble> CreateAllCoreComponents();

        protected void CreateComponent<TCrossBehaviour>() where TCrossBehaviour : IInitializeble, new()
        {
            _coreComponents[typeof(TCrossBehaviour)] = new TCrossBehaviour();
        }
    }
}
