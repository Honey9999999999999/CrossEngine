namespace CrossEngine.System
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        protected static T? _instance;

        public Singleton()
        {
            if(_instance != null)
            {
                throw new Exception($"{_instance.GetType()} has already initialized");
            }

            _instance = (T)this;
        }
    }
}
