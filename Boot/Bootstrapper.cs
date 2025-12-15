using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RSG
{
    [DefaultExecutionOrder(-100)]
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("Raised when all systems are loaded and ready.")]
        [SerializeField] private BootChannelSO m_systemsReadyChannel;

        [Header("Systems")]
        [Tooltip("Core systems to spawn. Sorted by priority automatically.")]
        [SerializeField] private List<BootSystem> m_bootPrefabs = new List<BootSystem>();

#if RSG_DEBUG
        [Header("Debug")]
        [SerializeField] private List<GameObject> m_debugPrefabs = new List<GameObject>();
#endif

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            List<BootSystem> initializedSystems = new List<BootSystem>();

            foreach (BootSystem prefab in m_bootPrefabs)
            {
                if (!prefab) continue;
                
                BootSystem instance = Instantiate(prefab, transform);
                initializedSystems.Add(instance);
            }

            // 2. Instantiate Debug Systems (if any)
#if RSG_DEBUG
            foreach (GameObject prefab in m_debugPrefabs)
            {
                if (prefab == null) continue;

                GameObject instance = Instantiate(prefab, transform);
                
                // Debug prefabs might be generic GameObjects, or they might be BootSystems.
                // Check if they need initialization.
                if (instance.TryGetComponent(out BootSystem bootSystem))
                {
                    initializedSystems.Add(bootSystem);
                }
            }
#endif

            // 3. Sort by Priority
            // We sort the *instances* here to ensure execution order is correct.
            IOrderedEnumerable<BootSystem> sortedSystems = initializedSystems.OrderBy(x => x.BootPriority);

            // 4. Initialize
            foreach (BootSystem system in sortedSystems)
            {
#if RSG_DEBUG
                Debug.Log($"[Bootstrapper] Initializing {system.GetType().Name} (Priority: {system.BootPriority})");
#endif
                system.Initialize();
            }

            // 5. Broadcast
            Debug.Log("[Bootstrapper] Initialization Complete. Raising Systems Ready.");
            if (m_systemsReadyChannel != null)
            {
                m_systemsReadyChannel.RaiseEvent();
            }
        }

        private void OnValidate()
        {
            // Keeps the inspector list tidy based on priority
            if (m_bootPrefabs.Count > 1)
            {
                m_bootPrefabs.Sort((a, b) => 
                {
                    if (!a || !b) return 0;
                    return a.BootPriority.CompareTo(b.BootPriority);
                });
            }
        }
    }
}