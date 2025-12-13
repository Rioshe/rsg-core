using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RSG
{
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Broadcasting To")]
        [Tooltip("Raised when all systems are loaded and ready.")]
        [SerializeField] private SystemReadyChannelSO m_systemsReadyChannel;

        [Header("System prefabs to auto-spawn")]
        [SerializeField] private List<GameObject> m_bootPrefabs = new List<GameObject>();

#if RSG_DEBUG
        [Header("Debug-only prefabs")]
        [SerializeField] private List<GameObject> m_debugPrefabs = new List<GameObject>();
#endif

        // CHANGED: Awake cannot be IEnumerator. Logic moved to Start.
        private void Awake()
        {
            // We can still spawn them in Awake to ensure they exist immediately
            // But we wait until Start to initialize them nicely
        }

        private void Start()
        {
            List<GameObject> instantiatedSystems = SpawnSystems();
            List<IBootSystem> bootableSystems = new List<IBootSystem>();
            
            foreach (GameObject systemGO in instantiatedSystems)
            {
                IBootSystem[] bootables = systemGO.GetComponents<IBootSystem>();
                bootableSystems.AddRange(bootables);
            }

            List<IBootSystem> sortedSystems = bootableSystems.OrderBy(x => x.BootPriority).ToList();

            foreach (IBootSystem system in sortedSystems)
            {
#if RSG_DEBUG
                Debug.Log($"[Bootstrapper] Initializing {system.GetType().Name} (Priority: {system.BootPriority})");
#endif
                system.Initialize();
            }

            Debug.Log("[Bootstrapper] Initialization Complete. Raising Systems Ready.");
            
            // FIRE THE SIGNAL
            if (m_systemsReadyChannel)
            {
                m_systemsReadyChannel.RaiseEvent();
            }
        }

        private List<GameObject> SpawnSystems()
        {
            List<GameObject> allPrefabs = new List<GameObject>(m_bootPrefabs);

#if RSG_DEBUG
            allPrefabs.AddRange(m_debugPrefabs);
#endif

            List<GameObject> instantiatedList = new List<GameObject>();

            foreach (GameObject prefab in allPrefabs)
            {
                if (!prefab) continue;

                GameObject instance = Instantiate(prefab);
                instance.name = prefab.name;
                DontDestroyOnLoad(instance);
                instantiatedList.Add(instance);
            }

            return instantiatedList;
        }
    }
}