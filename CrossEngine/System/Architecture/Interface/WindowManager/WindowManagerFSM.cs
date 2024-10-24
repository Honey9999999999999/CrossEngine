using CrossEngine.System.Architecture.Interface;
using CrossEngine.System.FSM;
using CrossEngine.System.Interface;
using CrossEngine.System.Kernel;

namespace CrossEngine.System
{
    internal class WindowManagerFSM : FinalStateMachine<ConsoleWindow>
    {
        public WindowManagerFSM()
        {
            AddState(new SceneTreeWindow());
            AddState(new RenderWindow());

            CoreManager.OnLoaded += EnterIn<SceneTreeWindow>;
        }
    }
}
