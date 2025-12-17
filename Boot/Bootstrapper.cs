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
        [SerializeField] private List<BootSystemBase> m_bootPrefabs = new List<BootSystemBase>();

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
            List<BootSystemBase> initializedSystems = new List<BootSystemBase>();

            foreach (BootSystemBase prefab in m_bootPrefabs)
            {
                if (!prefab) continue;
                
                BootSystemBase instance = Instantiate(prefab, transform);
                initializedSystems.Add(instance);
            }

#if RSG_DEBUG
            foreach (GameObject prefab in m_debugPrefabs)
            {
                if (!prefab) continue;

                GameObject instance = Instantiate(prefab, transform);
                if (instance.TryGetComponent(out BootSystemBase bootSystem))
                {
                    initializedSystems.Add(bootSystem);
                }
            }
#endif

            foreach (BootSystemBase system in initializedSystems)
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