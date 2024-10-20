using CrossEngine.Objects;
using CrossEngine.Render;
using CrossEngine.System;
using SharpHook.Native;
using System.Collections;
using System.Numerics;

namespace CrossEngine
{
    internal class TestScript : CrossBehaviour
    {
        public Camera camera;
        private float DeltaTime;
        private DateTime _oldtime;

        public override void Start()
        {
            base.Start();

            camera = new();
            camera.AddComponent<TestScript2>();

            //StartCoroutine(Hi());
            StartCoroutine(Render());
            //StartCoroutine(Stop());
        }

        public override void Update()
        {
            base.Update();

            if (Input.GetKey(KeyCode.VcD))
            {
                camera.Transform.Rotation += Vector3.UnitZ * Time.DeltaTime;
            }
            if (Input.GetKey(KeyCode.VcA))
            {
                camera.Transform.Rotation -= Vector3.UnitZ * Time.DeltaTime;
            }

            camera.Transform.Rotation += Vector3.UnitZ * Time.DeltaTime;
        }

        IEnumerator Render()
        {
            //camera.Transform.Position = new(0, -2, -1);
            //camera.Transform.Rotation = new(-63.4349f / 180 * MathF.PI, 0, 0);

            camera.Transform.Position = new(0, -5, 0);
            camera.Transform.Rotation = new(MathF.PI / 2, 0, 0);

            ConsoleScreen screen = new(240, 60, 8, 4);
            ConsoleRenderer renderer = new(screen, ref camera);

            List<Sphere> list = [new(), new(), new()];

            Vector3 pos = list[0].Transform.Position;
            // sphere.Transform.Position = -Vector3.UnitZ * 5;

            list[0].Transform.Position = new Vector3(3, 0, 0);
            list[1].Transform.Position = new Vector3(0, -15, 0);

            DateTime start = DateTime.UtcNow;
            int frames = 0;
            while (true)
            {
                Console.Title = $"FPS: {-1d / (start - (start = DateTime.UtcNow)).TotalSeconds:0.00}";
                list[0].Transform.Scale = Vector3.One * (MathF.Cos(frames++ * 0.01f) * 0.5f + 1);
                list[0].Transform.Position = pos + new Vector3(1, 0, 0) * (MathF.Sin(frames++ * 0.01f));

                ConsoleOutput.Write(renderer.Render([.. list]));

                Console.SetCursorPosition(0, 0);
                Console.Write(Debug.Tree.FromObject(camera, Debug.Tree.Config.PublicFields));
                //Console.Write(Debug.Tree.FromObject(camera.Transform.Rotation));

                //yield return new WaitForSeconds(0.01);
                yield return null;
            }
        }
    }    
}
