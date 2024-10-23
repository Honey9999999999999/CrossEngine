using CrossEngine.Objects;
using CrossEngine.Render;
using CrossEngine.System.Interface;
using System.Numerics;

namespace CrossEngine.System.Architecture.Interface
{
    internal class RenderWindow : ConsoleWindow
    {
        public RenderWindow() : base(new Vector2(25, 0), new Vector2(ConsoleScreen.instance.Width - 26, ConsoleScreen.instance.Height)) { }

        protected override CharInfo[] BuildCharArray()
        {
            Sphere[] sphereList = UpdateSphereList();

            return ConsoleRenderer.instance.Render([.. sphereList]);
        }

        private Sphere[] UpdateSphereList() => SceneManager.GetActiveScene().GetAllComponents<Sphere>(SceneManager.GetActiveScene().RootNode.Transform.GetChilds());
    }
}
