namespace CrossEngine.System.Core
{
    public interface ICoreConfig
    {
        public Dictionary<Type, ICrossBehaviour> CreateAllCrossBehaviours();
    }
}
