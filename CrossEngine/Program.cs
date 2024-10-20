using CrossEngine;
using CrossEngine.Objects;
using CrossEngine.System;
using CrossEngine.System.Kernel;


new Engine();
Engine.StartCore(TypeConfig.ConsoleCore);

GameObject starter = new("Starter");
starter.AddComponent<TestScript>();

GameObject debuger = new("Debuger", starter.Transform);
debuger.AddComponent<SceneTree>();

GameObject debuger1 = new("Debuger@", debuger.Transform);
debuger1.AddComponent<Sphere>();

GameObject debuger2 = new("DebugerAlpha", debuger.Transform);
debuger1.AddComponent<Sphere>();

GameObject debuger3 = new("DebugerBeta", debuger.Transform);
debuger1.AddComponent<Sphere>();

GameObject debuger4 = new("DebugerKurwa", debuger.Transform);
debuger1.AddComponent<Sphere>();

GameObject debuger5 = new("DebugerOmega", debuger1.Transform);
debuger1.AddComponent<Sphere>();

Engine.RunPlayMode();