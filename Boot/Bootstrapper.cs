using System.Collections.Generic;
using UnityEngine;

namespace RSG
{
    [DefaultExecutionOrder(-100)]
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("Raised when all systems are loaded and ready.")]
        [SerializeField] private BootChannelSO m_bootChannelSo;

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

#if RSG_DEBUG
            foreach (GameObject prefab in m_debugPrefabs)
            {
                if (!prefab) continue;

                GameObject instance = Instantiate(prefab, transform);
                if (instance.TryGetComponent(out BootSystem bootSystem))
                {
                    initializedSystems.Add(bootSystem);
                }
            }
#endif

            foreach (BootSystem system in initializedSystems)
            {
                system.Initialize();
            }

            if (m_bootChannelSo)
            {
                m_bootChannelSo.RaiseBootCompleteEvent();
            }
        }
    }
}