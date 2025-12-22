using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

namespace RSG
{
    [DefaultExecutionOrder(-100)]
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private string m_mainSceneId = "_Main";

        [Header("Systems")]
        [SerializeField] private List<BootSystemBase> m_bootPrefabs = new List<BootSystemBase>();

        private SplashSystem m_splashSystem;
        private SceneSystem m_sceneSystem;
        private TransitionSystem m_transitionSystem;

        private void Awake() => DontDestroyOnLoad(gameObject);

        private async void Start()
        {
            List<BootSystemBase> spawnedSystems = SpawnSystems();
            CacheSystemReferences(spawnedSystems);
            spawnedSystems.Sort((a, b) => a.InitializationPriority.CompareTo(b.InitializationPriority));

            Task splashTimerTask = Task.CompletedTask;
            if (m_splashSystem)
            {
                splashTimerTask = m_splashSystem.PlaySplashSequenceAsync();
            }

            await InitializeSystemsAsync(spawnedSystems);
            await splashTimerTask;
            await RunVisualSequenceAsync();
        }

        private List<BootSystemBase> SpawnSystems()
        {
            List<BootSystemBase> systems = new List<BootSystemBase>(m_bootPrefabs.Count);
            foreach (BootSystemBase prefab in m_bootPrefabs)
            {
                if (prefab) systems.Add(Instantiate(prefab, transform));
            }
            return systems;
        }

        private void CacheSystemReferences(List<BootSystemBase> systems)
        {
            m_splashSystem = GetSystem<SplashSystem>(systems);
            m_sceneSystem = GetSystem<SceneSystem>(systems);
            m_transitionSystem = GetSystem<TransitionSystem>(systems);
            
            CoreServices.Register( m_splashSystem );
            CoreServices.Register( m_sceneSystem );
            CoreServices.Register( m_transitionSystem );
            
            OnSystemsCreated(systems);
        }
        
        protected virtual void OnSystemsCreated(List<BootSystemBase> systems) { }

        private async Task InitializeSystemsAsync(List<BootSystemBase> systems)
        {
            foreach (BootSystemBase system in systems)
            {
                system.Initialize();
                await system.InitializeAsync();
            }
        }

        private async Task RunVisualSequenceAsync()
        {
            if (m_transitionSystem)
                await m_transitionSystem.ShowLoadingAsync();

            if (m_splashSystem)
                await m_splashSystem.FadeOutAsync();

            if (m_sceneSystem)
                await m_sceneSystem.LoadSceneAsync(m_mainSceneId);

            if (m_transitionSystem)
                await m_transitionSystem.HideLoadingAsync();
        }

        private void OnDestroy()
        {
            CoreServices.Clear();
            DestroySystems();
        }

        protected virtual void DestroySystems(){}

        protected static T GetSystem<T>(List<BootSystemBase> systems) where T : BootSystemBase => systems.FirstOrDefault(s => s is T) as T;
    }
}