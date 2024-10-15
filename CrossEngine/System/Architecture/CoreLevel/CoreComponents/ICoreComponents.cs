namespace CrossEngine.System.Core
{
    public interface ICoreComponents
    {
        public void SendOnCreateToAllCoreComponents();
        public void SendInitializeToAllCoreComponents();

        public TCoreComponent GetComponent<TCoreComponent>() where TCoreComponent : IInitializeble;
    }
}
