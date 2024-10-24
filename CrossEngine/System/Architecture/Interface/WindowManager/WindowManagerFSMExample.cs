using CrossEngine.System.FSM;
using CrossEngine.System.Interface;

namespace CrossEngine.System
{
    internal class WindowManagerFSMExample : FSMExample<WindowManagerFSM, ConsoleWindow>
    {
        public WindowManagerFSMExample()
        {
            Input.OnAnyKeyDown += Update;
        }

        public ConsoleWindow GetActiveWindow() => _stateMachine.CurrentState ?? throw new CrossException("CurrentState is null.");
        public Type[] GetTypes() => _stateMachine.GetKeys();
    }
}
