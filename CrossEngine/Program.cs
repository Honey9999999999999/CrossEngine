using CrossEngine;
using CrossEngine.System;
using CrossEngine.System.Kernel;


new Engine();
Engine.StartCore(TypeConfig.ConsoleCore);



Engine.RunPlayMode();

GameObject starter = new("Starter");
starter.AddComponent<TestScript>();