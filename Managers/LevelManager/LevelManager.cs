using System;
using UnityEngine;

namespace RSG
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        public static event Action<int> OnLevelChanged;
        public static event Action<int, int> OnStageChanged; // level, stage
        public static event Action<int> OnLevelCompleted;
        public static event Action<int, int> OnStageCompleted; // level, stage
        public static event Action<LevelConfig> OnLevelConfigLoaded;
        public static event Action<StageData> OnStageDataLoaded;

        private static int m_currentLevel = 1;
        private static int m_currentStage = 1;
        
        [Header("Configuration")]
        [SerializeField] private LevelDatabase m_levelDatabase;
        
        protected override void Init()
        {
        }

        public int GetStagesForLevel(int level)
        {
            if (m_levelDatabase == null)
            {
                Debug.LogError("[LevelManager] No LevelDatabase assigned!");
                return 0;
            }
            
            return m_levelDatabase.GetStagesForLevel(level);
        }
        
        public int GetTotalLevels()
        {
            if (!m_levelDatabase)
            {
                Debug.LogError("[LevelManager] No LevelDatabase assigned!");
                return 0;
            }
            
            return m_levelDatabase.TotalLevels;
        }
        
        public LevelConfig GetCurrentLevelConfig()
        {
            if (m_levelDatabase)
            {
                return m_levelDatabase.GetLevel(m_currentLevel);
            }
            
            return null;
        }
        
        public StageData GetCurrentStageData()
        {
            if (m_levelDatabase)
            {
                return m_levelDatabase.GetStageData(m_currentLevel, m_currentStage);
            }
            
            return null;
        }
        
        public LevelConfig GetLevelConfig(int level)
        {
            if (m_levelDatabase)
            {
                return m_levelDatabase.GetLevel(level);
            }
            
            return null;
        }
        
        public StageData GetStageData(int level, int stage)
        {
            if (m_levelDatabase)
            {
                return m_levelDatabase.GetStageData(level, stage);
            }
            
            return null;
        }
        
        public void CompleteStage()
        {
            Debug.Log($"[LevelManager] Stage {m_currentStage} of Level {m_currentLevel} completed.");
            
            OnStageCompleted?.Invoke(m_currentLevel, m_currentStage);
            
            int totalStages = GetStagesForLevel(m_currentLevel);
            
            if (m_currentStage < totalStages)
            {
                SetStage(m_currentStage + 1);
            }
            else
            {
                CompleteLevel();
            }
        }
        
        public void CompleteLevel()
        {
            Debug.Log($"[LevelManager] Level {m_currentLevel} completed!");
            
            OnLevelCompleted?.Invoke(m_currentLevel);
            
            if (m_currentLevel < GetTotalLevels())
            {
                SetLevel(m_currentLevel + 1);
            }
            else
            {
                Debug.Log($"[LevelManager] All levels completed!");
            }
        }
        
        public void SetLevel(int level)
        {
            if (level < 1 || level > GetTotalLevels())
            {
                Debug.LogError($"[LevelManager] Cannot set level to {level}. Valid range: 1-{GetTotalLevels()}");
                return;
            }
            
            if (m_currentLevel != level)
            {
                m_currentLevel = level;
                m_currentStage = 1;
                
                Debug.Log($"[LevelManager] Level set to {m_currentLevel}");
                
                OnLevelChanged?.Invoke(m_currentLevel);
                OnStageChanged?.Invoke(m_currentLevel, m_currentStage);
                
                if (m_levelDatabase)
                {
                    LevelConfig levelConfig = GetCurrentLevelConfig();
                    if (levelConfig)
                    {
                        OnLevelConfigLoaded?.Invoke(levelConfig);
                    }
                    
                    StageData stageData = GetCurrentStageData();
                    if (stageData != null)
                    {
                        OnStageDataLoaded?.Invoke(stageData);
                    }
                }
            }
        }
        
        public void SetStage(int stage)
        {
            int totalStages = GetStagesForLevel(m_currentLevel);
            
            if (stage < 1 || stage > totalStages)
            {
                Debug.LogError($"[LevelManager] Cannot set stage to {stage}. Valid range for level {m_currentLevel}: 1-{totalStages}");
                return;
            }
            
            if (m_currentStage != stage)
            {
                m_currentStage = stage;
                
                Debug.Log($"[LevelManager] Stage set to {m_currentStage} in Level {m_currentLevel}");
                
                OnStageChanged?.Invoke(m_currentLevel, m_currentStage);
                
                if (m_levelDatabase)
                {
                    StageData stageData = GetCurrentStageData();
                    if (stageData != null)
                    {
                        OnStageDataLoaded?.Invoke(stageData);
                    }
                }
            }
        }
        
        public void ResetProgress()
        {
            Debug.Log("[LevelManager] Resetting progress to Level 1, Stage 1");
            
            m_currentLevel = 1;
            m_currentStage = 1;
            
            OnLevelChanged?.Invoke(m_currentLevel);
            OnStageChanged?.Invoke(m_currentLevel, m_currentStage);
        }
        
        public void RestartStage()
        {
            Debug.Log($"[LevelManager] Restarting Stage {m_currentStage} of Level {m_currentLevel}");
            OnStageChanged?.Invoke(m_currentLevel, m_currentStage);
        }
        
        public void RestartLevel()
        {
            Debug.Log($"[LevelManager] Restarting Level {m_currentLevel}");
            SetStage(1);
        }
        
        public bool IsLastStageOfLevel()
        {
            return m_currentStage == GetStagesForLevel(m_currentLevel);
        }
        
        public bool IsLastLevel()
        {
            return m_currentLevel == GetTotalLevels();
        }
    }
}