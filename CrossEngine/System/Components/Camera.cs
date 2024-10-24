using CrossEngine.System;

namespace CrossEngine
{
    public class Camera : Component
    {
        public static Camera? Main { get; private set; }
        internal static Camera EditorCamera => _editorCamera ?? CreateEditorCamera();
        internal static Camera? _editorCamera;

        public float RayLength = 1;

        private static Camera CreateEditorCamera()
        {
            GameObject camera = new(SceneManager.GetActiveScene().EditorNode.Transform);
            _editorCamera = camera.AddComponent<Camera>();

            return camera.GetComponent<Camera>();
        }

        public void MakeMain() => Main = this;
    }
}
