using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RSG
{
    public class Bootstrapper : MonoBehaviour
    {
        [Header( "Scene to load after booting" )]
        [SerializeField] private string nextSceneName = "_Main";

        [Header( "System prefabs to auto-spawn" )]
        [SerializeField] private List<GameObject> systemPrefabs;
        [SerializeField] private List<GameObject> debugPrefabs;

        private static bool _initialized = false;

        private void Awake()
        {
            // Prevent duplicates if Boot scene is reloaded
            if( _initialized )
            {
                Destroy( gameObject );
                return;
            }

            _initialized = true;
            DontDestroyOnLoad( gameObject );

            CreateSystems();
            LoadNextScene();
        }

        /// <summary>
        /// Instantiates and preserves system prefabs between scenes.
        /// </summary>
        private void CreateSystems()
        {
#if RSG_DEBUG
            if( debugPrefabs != null && debugPrefabs.Count != 0 )
            {
                systemPrefabs.AddRange( debugPrefabs );
            }     
#endif
            
            foreach( GameObject prefab in systemPrefabs )
            {
                if( prefab == null )
                {
                    Debug.LogWarning( "[Bootstrapper] A system prefab slot is empty." );
                    continue;
                }

                GameObject instance = Instantiate( prefab );
                instance.name = prefab.name; // Remove (Clone)
                DontDestroyOnLoad( instance );
            }

            Debug.Log( $"[Bootstrapper] Spawned {systemPrefabs.Count} system prefabs." );
        }

        private void LoadNextScene()
        {
            Debug.Log( $"[Bootstrapper] Loading next scene: {nextSceneName}" );
            SceneManager.LoadSceneAsync( nextSceneName );
        }
    }
}