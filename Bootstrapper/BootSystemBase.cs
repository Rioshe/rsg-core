using System.Threading.Tasks;
using UnityEngine;

namespace RSG
{
    public abstract class BootSystemBase : MonoBehaviour
    {
        [SerializeField] private int m_initializationPriority = 0;
        public int InitializationPriority => m_initializationPriority;

        public abstract void Initialize();

        public virtual Task InitializeAsync() { return Task.CompletedTask; }

        public virtual void Shutdown() { }

        protected virtual void OnValidate()
        {
            string systemName = GetType().Name;
            if (gameObject.name != systemName)
            {
                gameObject.name = systemName;
            }

            ValidateDependencies();
        }

        protected virtual void ValidateDependencies() { }
    }
}