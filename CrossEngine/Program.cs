using CrossEngine;
using CrossEngine.System;
using CrossEngine.System.Kernel;


new Engine();
Engine.StartCore(TypeConfig.ConsoleCore);

GameObject starter = new();
SceneManager.GetActiveScene().AddRootObject(starter);

starter.AddComponent<TestScript>();

Engine.RunPlayMode();