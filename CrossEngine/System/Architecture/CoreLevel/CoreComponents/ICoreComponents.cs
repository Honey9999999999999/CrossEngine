namespace CrossEngine.System.Arhitecture.BaseLevel.CoreBase
{
    public interface ICoreComponents
    {
        public void SendOnCreateToAllCoreComponents();
        public void SendInitializeToAllCoreComponents();
        public void SendAwakeToAllCoreComponents();
        public void SendStartToAllCoreComponents();
        public void SendUpdateToAllCoreComponents();
        public void SendFixedUpdateToAllCoreComponents();

        public TCoreComponent GetComponent<TCoreComponent>() where TCoreComponent : CrossBehaviour;
    }
}
