using CrossEngine.System.Interfaces;

namespace CrossEngine.System
{
    public abstract class CoreComponentsConfig
    {
        protected Dictionary<Type, ICrossBehaviour> _coreComponents;

        public abstract Dictionary<Type, ICrossBehaviour> CreateAllCoreComponents();

        protected void CreateComponent<TCrossBehaviour>() where TCrossBehaviour : ICrossBehaviour, new()
        {
            _coreComponents[typeof(TCrossBehaviour)] = new TCrossBehaviour();
        }
    }
}
