namespace RSG.Core
{
    public abstract class Singleton<T> where T : class, new()
    {
        private static readonly object s_lock = new object();
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    lock (s_lock)
                    {
                        if (s_instance == null)
                            s_instance = new T();
                    }
                }
                return s_instance;
            }
        }

        protected Singleton()
        {
            // Prevent external instantiation through reflection
            if (s_instance != null)
                throw new System.Exception($"Singleton of type {typeof(T)} already exists!");
        }
    }
}