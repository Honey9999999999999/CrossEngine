namespace CrossEngine.System.Architecture.Scene
{
    internal interface ISceneManager
    {
        public event Action? OnSceneLoaded;
        public event Action? OnSceneUpLoaded;

        public int sceneCount { get; }

        public void CreateScene(string name);
        public IScene GetActiveScene();
        public IScene GetSceneAt(int index);
        public IScene GetSceneByName(string name);
        public void LoadScene(int index);
        public void LoadScene(string name);
    }
}
