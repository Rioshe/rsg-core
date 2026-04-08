using UnityEngine;

namespace RSG
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        [SerializeField] private bool m_unparent = true;
        
        private static T s_instance;
        private static readonly object s_lock = new object();
        private static bool s_isQuitting = false;

        public static T Instance
        {
            get
            {
                if (s_isQuitting)
                {
                    return null;
                }

                lock (s_lock)
                {
                    if (s_instance == null)
                    {
                        s_instance = FindFirstObjectByType(typeof(T)) as T;
                        if (!s_instance)
                        {
                            GameObject singletonObject = new GameObject();
                            s_instance = singletonObject.AddComponent<T>();
                            singletonObject.name = $"{typeof(T).ToString()} (Singleton)";
                        }
                    }
                    return s_instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this as T;
                if( m_unparent )
                {
                    transform.parent = null;
                }
                DontDestroyOnLoad(gameObject);
                Init(); 
            }
            else if (s_instance != this)
            {
                Debug.LogWarning($"Duplicate instance of {GetType()} found. Destroying...", gameObject);
                Destroy(gameObject);
            }
        }

        protected virtual void Init() { }
        protected virtual void OnApplicationQuit()
        {
            s_isQuitting = true;
        }
    }
}