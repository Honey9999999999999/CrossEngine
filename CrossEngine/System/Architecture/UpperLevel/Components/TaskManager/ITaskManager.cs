namespace CrossEngine
{
    public interface ITaskManager
    {
        public void AddTask(ITask task);
        public Stack<ITask> GetTasks();

        public void RunTasks();
    }
}
