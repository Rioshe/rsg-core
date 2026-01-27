using System.Threading.Tasks;
using UnityEngine;

namespace RSG
{
    public class LevelService : MonoBehaviour, ILevelService
    {
        [Header("Data")]
        [SerializeField] private LevelListSO m_levelList;
        [SerializeField] private PrefsDataSO m_prefsData;
        
        [Header("Spawning")]
        [SerializeField] private Transform m_levelAnchor;
        
        private Slicable m_currentLevelInstance;
        
        
        public Task InitializeAsync()
        {
            LevelEvents.OnSpawnCurrentLevel += SpawnCurrentLevel;
            ServiceLocator.Register<ILevelService>(this);
            return Task.CompletedTask;
        }
        
        public Task ShutdownAsync()
        {
            LevelEvents.OnSpawnCurrentLevel -= SpawnCurrentLevel;
            ServiceLocator.Unregister<ILevelService>(this);
            return Task.CompletedTask;
        }

        private void SpawnCurrentLevel()
        {
            if (m_currentLevelInstance)
                Destroy(m_currentLevelInstance.gameObject);

            int index = m_prefsData.CurrentLevelIndex;
            
            if (index < 0 || index >= m_levelList.Levels.Count)
            {
                Debug.LogError($"[LevelSystem] Index {index} out of range.");
                return;
            }
            
            LevelDataSO data = m_levelList.Levels[index];

            // Parent to anchor if it exists, otherwise root
            m_currentLevelInstance = Instantiate(data.SlicablePrefab, m_levelAnchor);
            m_currentLevelInstance.transform.localPosition = Vector3.zero;
            
            LevelEvents.RaiseLevelSpawned(data);
        }
    }
}