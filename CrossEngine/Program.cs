using CrossEngine;
using CrossEngine.Objects;
using CrossEngine.System;
using CrossEngine.System.Kernel;
using System.Numerics;


new Engine();
Engine.StartCore(TypeConfig.ConsoleCore);

GameObject starter = new("Starter");
starter.AddComponent<TestScript>();

GameObject debuger = new("Debuger", starter.Transform);
debuger.AddComponent<SceneTree>();

GameObject debuger1 = new("Debuger@", debuger.Transform);
debuger1.AddComponent<Sphere>();
debuger1.Transform.Position = new Vector3(7, 36, 0);

GameObject debuger2 = new("DebugerAlpha", debuger.Transform);
debuger2.AddComponent<Sphere>();
debuger2.Transform.Position = new Vector3(13, 66, 0);

GameObject debuger3 = new("DebugerBeta", debuger.Transform);
debuger3.AddComponent<Sphere>();
debuger3.Transform.Position = new Vector3(-13, -30, 0);

GameObject debuger4 = new("DebugerKurwa", debuger.Transform);
debuger4.AddComponent<Sphere>();
debuger4.Transform.Position = new Vector3(13, 0, 10);

GameObject debuger5 = new("DebugerOmega", debuger1.Transform);
debuger5.AddComponent<Sphere>();
debuger5.Transform.Position = new Vector3(63, 0, -70);

Engine.RunPlayMode();