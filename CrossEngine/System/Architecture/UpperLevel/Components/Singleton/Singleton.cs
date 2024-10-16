namespace CrossEngine.System
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new CrossException($"{typeof(T)} is not initialized!!!");
                }

                return _instance;
            }
        }

        private static T? _instance;

        public Singleton()
        {
            if(_instance != null)
            {
                throw new CrossException($"{_instance.GetType()} was be initialized");
            }

            _instance = (T)this;
        }
    }
}
