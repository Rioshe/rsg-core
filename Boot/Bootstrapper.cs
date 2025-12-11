using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RSG
{
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Scene to load after booting")]
        [SerializeField] private string m_nextSceneName = "";

        [Header("System prefabs to auto-spawn")]
        [SerializeField] private List<GameObject> m_bootPrefabs = new List<GameObject>();

#if RSG_DEBUG
        [Header("Debug-only prefabs")]
        [SerializeField] private List<GameObject> m_debugPrefabs = new List<GameObject>();
#endif

        private void Awake()
        {
            CreateSystems();
        }

        private void Start()
        {
            LoadNextScene();
        }
        
        // -------------------------------------------------------------------
        // SYSTEM CREATION
        // --------------------------------------------------------------------
        private void CreateSystems()
        {
            List<GameObject> systemsToSpawn = new List<GameObject>(m_bootPrefabs);

#if RSG_DEBUG
            systemsToSpawn.AddRange(m_debugPrefabs);
#endif

            foreach (GameObject prefab in systemsToSpawn)
            {
                if (!prefab)
                {
                    Debug.LogWarning("[Bootstrapper] Null prefab in list.");
                    continue;
                }

                GameObject instance = Instantiate(prefab);
                instance.name = prefab.name;

                DontDestroyOnLoad(instance);
            }

            Debug.Log($"[Bootstrapper] Spawned {systemsToSpawn.Count} bootstrap systems.");
        }


        // --------------------------------------------------------------------
        // SCENE LOADING
        // --------------------------------------------------------------------
        private void LoadNextScene()
        {
            if (string.IsNullOrEmpty(m_nextSceneName))
            {
                return;
            }

            Debug.Log($"[Bootstrapper] Loading next scene: {m_nextSceneName}");
            SceneManager.LoadSceneAsync(m_nextSceneName);
        }
    }
}
