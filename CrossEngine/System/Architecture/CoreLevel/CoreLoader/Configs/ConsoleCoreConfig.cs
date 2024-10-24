using CrossEngine.Render;

namespace CrossEngine.System.Kernel
{
    internal class ConsoleCoreConfig : CoreLoaderConfig
    {
        protected override void CreateAllCoreComponents()
        {
            CreateComponent<FileManager>();            
            CreateComponent<SceneManager>();
            CreateComponent<SceneTree>();

            CreateComponent<Time>();
            CreateComponent<Input>(); //Создает якорь, работает в асинхроне, отцепить якорь при закрытии приложения

            CreateComponent<ConsoleScreen>();
            CreateComponent<ConsoleRenderer>();
            CreateComponent<WindowManager>();            
        }
    }
}
