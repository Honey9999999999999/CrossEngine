namespace CrossEngine.System.Interfaces
{
    public interface ICoreConfig
    {
        public Dictionary<Type, ICrossBehaviour> CreateAllCrossBehaviours();
    }
}
