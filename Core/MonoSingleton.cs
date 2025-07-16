using UnityEngine;

namespace RSG.Core
{
	public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
	{
		private static T s_instance;
		private static bool s_isInitialised;

		public static T Instance
		{
			get
			{
				if(!s_instance)
				{
					s_instance = FindFirstObjectByType(typeof(T)) as T;
					if(!s_instance)
					{
						s_instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();

						if(!s_instance)
						{
							Debug.LogError("Problem during the creation of " + typeof(T).ToString());
						}
					}
                
					if (!s_isInitialised){
						s_isInitialised = true;
						s_instance.Init();
					}
				}
            
				return s_instance;
			}
		}

		private void Awake()
		{
			if (!s_instance) {
				s_instance = this as T;
			} else if (s_instance != this) {
				Debug.LogError ("Another instance of " + GetType () + " is already exist! Destroying self...");
				DestroyImmediate (this);
				return;
			}
			if (!s_isInitialised) {
				DontDestroyOnLoad(gameObject);
				s_isInitialised = true;
				if (s_instance) s_instance.Init();
			}
		}
    
		public virtual void Init(){}
	
		private void OnApplicationQuit()
		{
			s_instance = null;
		}
	}
}
