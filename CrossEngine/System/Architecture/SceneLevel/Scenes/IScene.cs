namespace CrossEngine.System
{
    internal interface IScene
    {
        public string name { get; }
        public int index { get; }
        public int rootCount { get; }

        public List<CrossBehaviour> GetRootGameObjects();
        public void AddRootObject(CrossBehaviour rootObject);
    }
}
