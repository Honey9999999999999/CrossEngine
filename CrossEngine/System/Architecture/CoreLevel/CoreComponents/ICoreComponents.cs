namespace CrossEngine.System.Arhitecture.Core
{
    public interface ICoreComponents
    {
        public void SendOnCreateToAllCoreComponents();
        public void SendInitializeToAllCoreComponents();
        //public void SendAwakeToAllCoreComponents();
        //public void SendStartToAllCoreComponents();
        //public void SendUpdateToAllCoreComponents();
        //public void SendFixedUpdateToAllCoreComponents();

        public TCoreComponent GetComponent<TCoreComponent>() where TCoreComponent : IInitializeble;
    }
}
