namespace CrossEngine.System
{
    public class Task(Action action) : ITask
    {
        private readonly Action _action = action;

        public void Run()
        {
            _action.Invoke();
        }
    }
}
