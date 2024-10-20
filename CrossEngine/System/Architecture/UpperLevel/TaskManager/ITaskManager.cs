namespace CrossEngine.System
{
    public interface ITaskManager
    {
        public void AddTask(ITask task);
        public ITask[] GetTasks();

        public void RunTasks();
    }
}
