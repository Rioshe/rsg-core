using UnityEngine;

namespace RSG
{
    public abstract class BootSystemBase : MonoBehaviour
    {
        public abstract void Initialize();

        protected virtual void OnValidate()
        {
            // Auto-rename the GameObject to match the class name for cleaner hierarchy
            string systemName = GetType().Name;
            if (gameObject.name != systemName)
            {
                gameObject.name = systemName;
            }
        }
    }
}