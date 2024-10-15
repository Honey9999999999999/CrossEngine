namespace CrossEngine.System
{
    internal interface ISceneManager
    {
        public static abstract event Action? OnSceneStarted;
        public static abstract event Action? OnSceneStoped;
        public static abstract event Action? OnSceneLoaded;

        public static int sceneCount { get; }

        public static abstract void CreateScene(string name);

        public static abstract SceneBase GetActiveScene();
        public static abstract SceneBase GetSceneAt(int index);
        public static abstract SceneBase GetSceneByName(string name);

        public static abstract void StartActiveScene();

        public static abstract void LoadScene(SceneBase scene);
        public static abstract void LoadScene(int index);
        public static abstract void LoadScene(string name);
    }
}
