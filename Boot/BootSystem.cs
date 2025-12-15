using UnityEngine;

namespace RSG
{
    public abstract class BootSystem : MonoBehaviour
    {
        [Tooltip("Lower values initialize first.")]
        [SerializeField] private int m_bootPriority = 0;

        public int BootPriority => m_bootPriority;

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