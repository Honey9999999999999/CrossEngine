using CrossEngine;
using CrossEngine.Objects;
using CrossEngine.Render;
using CrossEngine.System;
using CrossEngine.System.Architecture.Scene;
using System.Collections;

Engine engine = new();
engine.StartCore();
engine.RunPlayMode();

GameObject starter = new();

SceneManager.instance.GetActiveScene().AddRootObject(starter);

starter.StartCoroutine(Hi());
starter.StartCoroutine(Render());
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
    Camera camera = new();
    camera.Transform.Position = new(5, 6, 3);
    camera.Transform.Rotation = new(68, 0, 140);
    
    ConsoleScreen screen = new(160, 40, 8, 4);
    Renderer renderer = new(screen, ref camera);

    Sphere sphere = new();

    while (true)
    {
        ConsoleOutput.Write(renderer.Render([sphere]));
        yield return null;
    }
}