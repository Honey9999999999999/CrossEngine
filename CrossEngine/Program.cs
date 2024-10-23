using CrossEngine;
using CrossEngine.System;
using CrossEngine.System.Architecture.Interface;
using CrossEngine.System.Kernel;


new Engine();
Engine.StartCore(TypeConfig.ConsoleCore);

GameObject starter = new("Starter");
starter.AddComponent<TestScript>();

GameObject debuger = new("Debuger");
//debuger.AddComponent<SceneTree>();

GameObject Sphere = new GameObject(debuger.Transform);

Console.Clear();

SceneTreeWindow treeWindow = new();

treeWindow.UpdateWithBounds();

Engine.RunPlayMode();