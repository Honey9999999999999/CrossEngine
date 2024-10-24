using CrossEngine.System.Interface;
using CrossEngine.System.Kernel;

namespace CrossEngine.System
{
    internal class WindowManager : CoreComponent<WindowManager>
    {
        private WindowManagerFSMExample _example;
        private Type[] _windows;

        private int _index;

        public WindowManager()
        {
            _example = new();
            _windows = _example.GetTypes();
        }

        public ConsoleWindow GetActiveWindow() => _example.GetActiveWindow();

        public void Next()
        {
            _index = _index < _windows.Length - 1 ? _index++ : 0;

            //instance._example.E
        }

        public ConsoleWindow GetWindow<TWindow>() where TWindow : ConsoleWindow
        {
            return _example.GetState<TWindow>();
        }
    }
}
