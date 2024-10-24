using CrossEngine.Objects;
using CrossEngine.Render;
using CrossEngine.System.Interface;
using System.Numerics;

namespace CrossEngine.System.Architecture.Interface
{
    internal class RenderWindow : ConsoleWindow
    {
        ConsoleRenderer renderer;

        public RenderWindow() : base(new Vector2(25, 0), 
            new Vector2(Engine.GetCoreComponent<ConsoleScreen>().Width - 26, Engine.GetCoreComponent<ConsoleScreen>().Height))
        {
            renderer = Engine.GetCoreComponent<ConsoleRenderer>();
        }

        public override void Update()
        {
        }

        protected override CharInfo[] BuildCharArray()
        {
            Sphere[] sphereList = UpdateSphereList();

            return renderer.Render([.. sphereList]);
        }

        private Sphere[] UpdateSphereList() => SceneManager.GetActiveScene().GetAllComponents<Sphere>(SceneManager.GetActiveScene().RootNode.Transform.GetChilds());
    }
}
