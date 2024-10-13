namespace CrossEngine
{
    public class Task(Action action) : ITask
    {
        private Action _action = action;

        public void Run()
        {
            _action.Invoke();
        }
    }
}
