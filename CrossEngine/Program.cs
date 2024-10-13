using CrossEngine;
using CrossEngine.Objects;
using CrossEngine.Render;
using CrossEngine.System;
using CrossEngine.System.Architecture.Scene;
using System.Collections;
using System.Numerics;

Camera camera = new();
Engine engine = new();
engine.StartCore();
engine.RunPlayMode();

GameObject starter = new();

SceneManager.instance.GetActiveScene().AddRootObject(starter);

// starter.StartCoroutine(Hi());
starter.StartCoroutine(Render());
//starter.StartCoroutine(Input());
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
    engine.StopPlayMode();
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
    // sphere.Transform.Position = -Vector3.UnitZ * 5;

    DateTime start = DateTime.UtcNow;
    int frames = 0;
    while (true)
    {
        Console.Title = $"FPS: {-1d / (start - (start = DateTime.UtcNow)).TotalSeconds:0}";
        sphere.Transform.Scale = Vector3.One * (MathF.Cos(frames++ * 0.01f) * 0.5f + 1);

        ConsoleOutput.Write(renderer.Render([sphere]));

        Console.SetCursorPosition(0, 0);
        Console.Write(Debug.Tree.FromObject(sphere, Debug.Tree.Config.PublicFields));
        Console.Write(Debug.Tree.FromObject(camera, Debug.Tree.Config.PublicFields));
        //Console.Write(Debug.Tree.FromObject(camera.Transform.Rotation));

        //yield return new WaitForSeconds(0.01);
        yield return null;
    }
}

IEnumerator Input()
{
    while (true)
    {
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.LeftArrow:
                camera.Transform.Rotation -= Vector3.UnitZ * 0.01f;
                break;
            case ConsoleKey.RightArrow:
                camera.Transform.Rotation += Vector3.UnitZ * 0.01f;
                break;
            case ConsoleKey.UpArrow:
                camera.Transform.Rotation += Vector3.UnitX * 0.01f;
                break;
            case ConsoleKey.DownArrow:
                camera.Transform.Rotation -= Vector3.UnitX * 0.01f;
                break;
        }
        yield return null;
    }
}