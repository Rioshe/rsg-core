using System.Collections.Generic;
using UnityEngine;

namespace RSG
{
    [DefaultExecutionOrder(-100)]
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private BootChannelSO m_bootChannelSo;

        [Header("Systems")]
        [SerializeField] private List<BootSystemBase> m_bootPrefabs = new List<BootSystemBase>();

#if RSG_DEBUG
        [Header("Debug")]
        [SerializeField] private List<GameObject> m_debugPrefabs = new List<GameObject>();
#endif

        private readonly List<BootSystemBase> m_initializedSystems = new List<BootSystemBase>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private async void Start()
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

            initializedSystems.Sort((a, b) => a.InitializationPriority.CompareTo(b.InitializationPriority));

            foreach (BootSystemBase system in initializedSystems)
            {
                system.Initialize();
                m_initializedSystems.Add(system);
            }

            foreach (BootSystemBase system in initializedSystems)
            {
                await system.InitializeAsync();
            }

            if (m_bootChannelSo)
            {
                m_bootChannelSo.RaiseBootCompleteEvent();
            }
        }

        private void OnDestroy()
        {
            foreach (BootSystemBase system in m_initializedSystems)
            {
                if (system)
                {
                    system.Shutdown();
                }
            }
        }
    }
}