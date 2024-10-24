using CrossEngine;
using CrossEngine.System;
using CrossEngine.System.Kernel;


Engine.Initialize(TypeConfig.ConsoleCore);

GameObject starter = new("Starter");
starter.AddComponent<TestScript>();

GameObject debuger = new("Debuger");
//debuger.AddComponent<SceneTree>();

GameObject Sphere = new GameObject(debuger.Transform);

Engine.RunPlayMode();