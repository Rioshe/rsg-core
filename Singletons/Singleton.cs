using System;

namespace RSG.Core
{
    public abstract class Singleton<T> where T : class
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
                        {
                            s_instance = (T)Activator.CreateInstance(typeof(T), true);
                        }
                    }
                }
                return s_instance;
            }
        }

        protected Singleton() { }
    }
}