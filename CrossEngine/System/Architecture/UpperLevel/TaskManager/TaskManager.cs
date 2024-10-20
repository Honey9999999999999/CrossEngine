namespace CrossEngine.System
{
    public class TaskManager : ITaskManager
    {
        private Queue<ITask> _tasks;

        public TaskManager()
        {
            _tasks = [];
        }

        public void AddTask(ITask task)
        {
            _tasks.Enqueue(task);
        }

        public ITask[] GetTasks()
        {
            return [.. _tasks];
        }

        public void RunTasks()
        {
            while (_tasks.TryDequeue(out ITask task))
            {
                task.Run();
            }
        }
    }
}
