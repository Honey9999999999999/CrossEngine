using CrossEngine;
using CrossEngine.System;
using CrossEngine.System.Kernel;

new Engine();
Engine.StartCore(TypeConfig.ConsoleCore);

GameObject starter = new();
SceneManager.GetActiveScene().AddRootObject(starter);

starter.AddComponent<TestScript>();
Console.WriteLine(starter.GetComponent<TestScript>().Enabled);

Engine.RunPlayMode();