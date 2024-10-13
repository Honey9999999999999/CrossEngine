using CrossEngine.System.Arhitecture.Core;

namespace CrossEngine.System
{
    public class Engine : EngineBase<CoreManagerExample>
    {
        public Engine() : this(TypeConfigs.ConsoleCore)
        {
        }

        public Engine(TypeConfigs type) : base(type)
        {
        }
    }
}
