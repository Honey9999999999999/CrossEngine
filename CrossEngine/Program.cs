using CrossEngine;
using CrossEngine.Objects;
using CrossEngine.System;
using CrossEngine.System.Kernel;
using System.Numerics;


new Engine();
Engine.StartCore(TypeConfig.ConsoleCore);

GameObject starter = new("Starter");
starter.AddComponent<TestScript>();

GameObject debuger = new("Debuger");
debuger.AddComponent<SceneTree>();

GameObject sphere1 = new("Debuger@", debuger.Transform);
sphere1.AddComponent<Sphere>();
sphere1.Transform.Position = new Vector3(5, 0, 0);

GameObject sphere2 = new("DebugerLOL", debuger.Transform);
sphere2.AddComponent<Sphere>();
sphere2.Transform.Position = new Vector3(5, 0, 1);

Engine.RunPlayMode();