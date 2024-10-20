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
        public GameObject camera;

        public override void Start()
        {
            base.Start();

            camera = new("Camera");
            camera.AddComponent<Camera>();

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
        }

        IEnumerator Render()
        {
            //camera.Transform.Position = new(0, -2, -1);
            //camera.Transform.Rotation = new(-63.4349f / 180 * MathF.PI, 0, 0);

            camera.Transform.Position = new(0, -5, 0);
            camera.Transform.Rotation = new(MathF.PI / 2, 0, 0);

            ConsoleScreen screen = new(240, 60, 8, 4);

            Camera cameraCom = camera.GetComponent<Camera>();
            ConsoleRenderer renderer = new(screen, ref cameraCom);

            List<GameObject> gameObjectList = [new("Sphere 1"), new("Sphere 2"), new("Sphere 3")];
            List<Sphere> sphereList = [];

            foreach(GameObject gameObject in gameObjectList)
            {
                gameObject.AddComponent<Sphere>();
                sphereList.Add(gameObject.GetComponent<Sphere>());
            }

            Vector3 pos = gameObjectList[0].Transform.Position;
            // sphere.Transform.Position = -Vector3.UnitZ * 5;

            gameObjectList[0].Transform.Position = new Vector3(3, 0, 0);
            gameObjectList[1].Transform.Position = new Vector3(0, -15, 0);

            DateTime start = DateTime.UtcNow;
            int frames = 0;
            while (true)
            {
                Console.Title = $"FPS: {-1d / (start - (start = DateTime.UtcNow)).TotalSeconds:0.00}";
                gameObjectList[0].Transform.Scale = Vector3.One * (MathF.Cos(frames++ * 0.01f) * 0.5f + 1);
                gameObjectList[0].Transform.Position = pos + new Vector3(1, 0, 0) * (MathF.Sin(frames++ * 0.01f));

                ConsoleOutput.Write(renderer.Render([.. sphereList]));

                Console.SetCursorPosition(0, 0);
                Console.Write(Debug.Tree.FromObject(camera, Debug.Tree.Config.PublicFields));
                //Console.Write(Debug.Tree.FromObject(camera.Transform.Rotation));

                //yield return new WaitForSeconds(0.01);
                yield return null;
            }
        }
    }    
}
