namespace CrossEngine
{
    public class TaskManager : ITaskManager
    {
        private Stack<ITask> _tasks;

        public TaskManager()
        {
            _tasks = [];
        }

        public void AddTask(ITask task)
        {
            _tasks.Push(task);
        }

        public Stack<ITask> GetTasks()
        {
            return _tasks;
        }

        public void RunTasks()
        {
            while (_tasks.TryPop(out ITask task))
            {
                task.Run();
            }
        }
    }
}
