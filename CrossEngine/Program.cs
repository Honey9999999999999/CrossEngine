using CrossEngine;
using CrossEngine.Objects;
using CrossEngine.Render;
using CrossEngine.System;
using CrossEngine.System.Kernel;
using System.Collections;
using System.Numerics;
using System.Text;

using System.Runtime.InteropServices;

//[DllImport("user32.dll")]
//static extern int MessageBox(IntPtr hWnd, String text, String caption, int options);

//MessageBox(IntPtr.Zero, "Hello", "My Message", 0);


//[DllImport("user32.dll")]
//static extern short GetAsyncKeyState(int keys);

//if (GetAsyncKeyState(53) != 0)
//    MessageBox(IntPtr.Zero, "клавиши нажаты", "клавиши нажаты", 0);
//else
//{
//    MessageBox(IntPtr.Zero, ":c", ":c", 0);
//}

Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

Camera camera = new();
new Engine();
Engine.StartCore(TypeConfig.ConsoleCore);
Engine.RunPlayMode();

//Stream inputStream = Console.OpenStandardInput();
//byte[] bytes = new byte[100];
//Console.WriteLine("To decode, type or paste the UTF8 encoded string and press enter:");
//Console.WriteLine("(Example: \"M+APw-nchen ist wundervoll\")");
//int outputLength = inputStream.Read(bytes, 0, 100);
//char[] chars = Encoding.UTF8.GetChars(bytes, 0, outputLength);
//Console.WriteLine($"Decoded string : {new string(chars)}");

GameObject starter = new();
SceneManager.GetActiveScene().AddRootObject(starter);


// starter.StartCoroutine(Hi());
starter.StartCoroutine(Render());
starter.StartCoroutine(InputRoutine());
//starter.StartCoroutine(Stop());

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

    Sphere sphere = new();
    Sphere sphere1 = new();

    Vector3 pos = sphere.Transform.Position;
    // sphere.Transform.Position = -Vector3.UnitZ * 5;

    sphere1.Transform.Position = new Vector3(3, 0, 0);

    DateTime start = DateTime.UtcNow;
    int frames = 0;
    while (true)
    {
        Console.Title = $"FPS: {-1d / (start - (start = DateTime.UtcNow)).TotalSeconds:0}";
        sphere.Transform.Scale = Vector3.One * (MathF.Cos(frames++ * 0.01f) * 0.5f + 1);
        sphere.Transform.Position = pos + new Vector3(1, 0, 0) * (MathF.Sin(frames++ * 0.01f));

        ConsoleOutput.Write(renderer.Render([sphere, sphere1]));

        Console.SetCursorPosition(0, 0);
        Console.Write(Debug.Tree.FromObject(sphere, Debug.Tree.Config.PublicFields));
        Console.Write(Debug.Tree.FromObject(camera, Debug.Tree.Config.PublicFields));
        //Console.Write(Debug.Tree.FromObject(camera.Transform.Rotation));

        //yield return new WaitForSeconds(0.01);
        yield return null;
    }
}

IEnumerator InputRoutine()
{
    while (true)
    {
        if (Input.GetKeyDown(ConsoleKey.D))
        {
            camera.Transform.Rotation += Vector3.UnitZ * 0.01f;
        }
        if (Input.GetKeyDown(ConsoleKey.A))
        {
            camera.Transform.Rotation -= Vector3.UnitZ * 0.01f;
        }
        yield return null;
    }
}