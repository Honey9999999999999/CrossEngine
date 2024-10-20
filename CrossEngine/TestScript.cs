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

            Console.WriteLine();
            foreach (var item in SceneManager.GetActiveScene().GetRootObjects())
            {
                Console.WriteLine($"RootObject : {item.GameObject.Name}");

                Component[] components = item.GetComponents();
                for (int i = 0; i < components.Length; i++)
                {
                    Console.CursorLeft = 4;
                    Console.WriteLine($"{(i == components.Length - 1 ? "└" : "├")}Object component : {components[i].GetType().Name}");
                }
            }

            DateTime timeTick = DateTime.UtcNow;
            DeltaTime = (float)(timeTick - _oldtime).TotalSeconds;
            _oldtime = timeTick;

            Console.WriteLine(1 / DeltaTime);
        }

        IEnumerator Hi()
        {
            string text = "Here you can find activities to practise your reading skills. Reading will help you to improve your understanding of the language and build your vocabulary.\r\n\r\nThe self-study lessons in this section are written and organised by English level based on the Common European Framework of Reference for languages (CEFR). There are different types of texts and interactive exercises that practise the reading skills you need to do well in your studies, to get ahead at work and to communicate in English in your free time.\r\n\r\nTake our free online English test to find out which level to choose. Select your level, from A1 English level (elementary) to C1 English level (advanced), and improve your reading skills at your own speed, whenever it's convenient for you.";

            foreach (var item in text)
            {
                Console.Write(item);
                yield return new WaitForSeconds(0.02d);
            }
        }
        IEnumerator Stop()
        {
            yield return new WaitForSeconds(4d);
            Engine.StopPlayMode();
        }

        IEnumerator Render()
        {
            //camera.Transform.Position = new(0, -2, -1);
            //camera.Transform.Rotation = new(-63.4349f / 180 * MathF.PI, 0, 0);

            camera.Transform.Position = new(0, -5, 0);
            camera.Transform.Rotation = new(MathF.PI / 2, 0, 0);

            ConsoleScreen screen = new(240, 60, 8, 4);
            Renderer renderer = new(screen, ref camera);

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
