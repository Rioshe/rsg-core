using UnityEngine;

namespace RSG
{
    public abstract class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_instance;
        public static T Instance
        {
            get
            {
                if (!s_instance)
                {
                    s_instance = FindAnyObjectByType<T>();
                    if (!s_instance)
                    {
                        Debug.LogError($"No instance of {typeof(T)} found in the scene.");
                    }
                }
                return s_instance;
            }
        }

        private void Awake()
        {
            if (!s_instance)
            {
                s_instance = this as T;
                if (s_instance) Init();
            }
            else if (s_instance != this)
            {
                Debug.LogWarning($"Multiple instances of {typeof(T)} found in scene. Destroying duplicate.");
                Destroy(gameObject);
            }
        }

        public virtual void Init(){}
        
        protected virtual void OnDestroy()
        {
            if (s_instance == this)
                s_instance = null;
        }
    }
}