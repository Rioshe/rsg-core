using System;
using UnityEngine;

namespace RSG
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        public static event Action<LevelConfigBase> OnLevelLoaded;
        public static event Action<LevelConfigBase> OnLevelCompleted;
        public static event Action OnAllLevelsCompleted;

        [Header("Configuration")]
        [SerializeField] private LevelDatabase m_levelDatabase;

        private int m_currentLevelIndex;
        public LevelConfigBase CurrentLevelConfig { get; private set; }


        #region Navigation
        public void LoadLevel(int levelIndex)
        {
            if (!m_levelDatabase)
            {
                Debug.LogError("[LevelManager] No Database assigned!");
                return;
            }

            if (levelIndex < 0 || levelIndex >= m_levelDatabase.TotalLevels)
            {
                Debug.LogError($"[LevelManager] Level index {levelIndex} out of range.");
                return;
            }

            m_currentLevelIndex = levelIndex;
            CurrentLevelConfig = m_levelDatabase.GetLevel(m_currentLevelIndex);
            
            Debug.Log($"[LevelManager] Level Loaded: {CurrentLevelConfig.LevelName}");
            OnLevelLoaded?.Invoke(CurrentLevelConfig);
        }

        public void CompleteLevel()
        {
            if (!CurrentLevelConfig) return;

            Debug.Log($"[LevelManager] Level {CurrentLevelConfig.LevelName} Complete!");
            OnLevelCompleted?.Invoke(CurrentLevelConfig);

            if (m_currentLevelIndex + 1 < m_levelDatabase.TotalLevels)
            {
                LoadLevel(m_currentLevelIndex + 1);
            }
            else
            {
                Debug.Log("[LevelManager] All Levels Completed!");
                OnAllLevelsCompleted?.Invoke();
            }
        }

        public void RestartLevel()
        {
            LoadLevel(m_currentLevelIndex);
        }
        #endregion
    }
}