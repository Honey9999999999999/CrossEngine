namespace CrossEngine.System.Arhitecture.Core
{
    public interface ICoreConfig
    {
        public Dictionary<Type, ICrossBehaviour> CreateAllCrossBehaviours();
    }
}
