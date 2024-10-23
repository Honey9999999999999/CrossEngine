using CrossEngine.Objects;
using CrossEngine.Render;
using CrossEngine.System.Architecture.Interface;
using SharpHook.Native;
using System.Collections;
using System.Numerics;

namespace CrossEngine
{
    internal class TestScript : CrossBehaviour
    {
        public GameObject camera;
        private int counter;

        public override void Start()
        {
            base.Start();

            camera = new("Camera");
            camera.AddComponent<Camera>();

            GameObject[] objects = [new("Sphere 1"), new("Sphere 2"), new("Sphere 2"), new("Sphere 2"), new("Sphere 2"), new("Sphere 2"), new("Sphere 2"), new("Sphere 2"), new("Sphere 2")];

            Random rnd = new();
            foreach (GameObject obj in objects)
            {
                obj.AddComponent<Sphere>();
                obj.Transform.Position = new((float)(rnd.NextDouble() - 0.5) * 5, 7, (float)(rnd.NextDouble() - 0.5) * 5);
            }

            objects[5].Transform.Position = new Vector3(0, -5, 3);

            //StartCoroutine(Hi());
            StartCoroutine(Render());
            //StartCoroutine(Stop());
        }

        public override void Update()
        {
            base.Update();

            if (Input.GetKey(KeyCode.VcD))
                camera.Transform.Rotation += Vector3.UnitZ * Time.DeltaTime;
            if (Input.GetKey(KeyCode.VcA))
                camera.Transform.Rotation -= Vector3.UnitZ * Time.DeltaTime;
            if (Input.GetKey(KeyCode.VcW))
                camera.Transform.Rotation += Vector3.UnitX * Time.DeltaTime;
            if (Input.GetKey(KeyCode.VcS))
                camera.Transform.Rotation -= Vector3.UnitX * Time.DeltaTime;
            if (Input.GetKey(KeyCode.VcQ))
                camera.Transform.Rotation += Vector3.UnitY * Time.DeltaTime;
            if (Input.GetKey(KeyCode.VcE))
                camera.Transform.Rotation -= Vector3.UnitY * Time.DeltaTime;

            if (Input.GetKey(KeyCode.VcUp))
            {
                camera.Transform.Position += camera.Transform.Forward * Time.DeltaTime;
            }
            if (Input.GetKey(KeyCode.VcDown))
            {
                camera.Transform.Position -= camera.Transform.Forward * Time.DeltaTime;
            }

            if (Input.GetKey(KeyCode.VcE))
            {
                if (GameObject.TryGetGameObjectWithName($"Test{counter}", out GameObject gameObject))
                {
                    _ = new GameObject($"Test{++counter}", gameObject.Transform);
                }
                else
                {
                    _ = new GameObject($"Test{counter}");
                }                
            }
        }

        IEnumerator Render()
        {
            camera.Transform.Position = new(0, -5, 0);
            camera.Transform.Rotation = new(MathF.PI / 2, 0, 0);

            Camera cameraCom = camera.GetComponent<Camera>();

            RenderWindow window = new();
            _ = new ConsoleRenderer(window, ref cameraCom);

            while (true)
            {
                window.UpdateWithBounds();
                yield return null;
            }
        }
    }
}
