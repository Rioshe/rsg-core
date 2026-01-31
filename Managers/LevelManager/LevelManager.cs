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
        
        public static int CurrentLevel { get; private set; } = 1;
        public static int CurrentStage { get; private set; } = 1;
        
        [Header("Configuration")]
        [SerializeField] private LevelDatabase m_levelDatabase;
        
        [Header("Progress Storage (Optional)")]
        [SerializeField] private bool m_enableProgressStorage = true;
        [SerializeField] private StorageType m_storageType = StorageType.PlayerPrefs;
        
        private ILevelProgressStorage m_progressStorage;
        
        public enum StorageType
        {
            None,
            PlayerPrefs,
            Memory
        }
        
        public LevelDatabase Database
        {
            get => m_levelDatabase;
        }

        protected override void Init()
        {
            InitializeStorage();
            LoadProgress();
        }
        
        private void InitializeStorage()
        {
            if (!m_enableProgressStorage)
            {
                m_progressStorage = null;
                return;
            }
            
            switch (m_storageType)
            {
                case StorageType.PlayerPrefs:
                    m_progressStorage = new PlayerPrefsLevelStorage();
                    break;
                case StorageType.Memory:
                    m_progressStorage = new MemoryLevelStorage();
                    break;
                case StorageType.None:
                default:
                    m_progressStorage = null;
                    break;
            }
        }
        
        public void SetCustomStorage(ILevelProgressStorage storage)
        {
            m_progressStorage = storage;
            LoadProgress();
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
            if (m_levelDatabase == null)
            {
                Debug.LogError("[LevelManager] No LevelDatabase assigned!");
                return 0;
            }
            
            return m_levelDatabase.TotalLevels;
        }
        
        public LevelConfig GetCurrentLevelConfig()
        {
            if (m_levelDatabase != null)
            {
                return m_levelDatabase.GetLevel(CurrentLevel);
            }
            
            return null;
        }
        
        public StageData GetCurrentStageData()
        {
            if (m_levelDatabase != null)
            {
                return m_levelDatabase.GetStageData(CurrentLevel, CurrentStage);
            }
            
            return null;
        }
        
        public LevelConfig GetLevelConfig(int level)
        {
            if (m_levelDatabase != null)
            {
                return m_levelDatabase.GetLevel(level);
            }
            
            return null;
        }
        
        public StageData GetStageData(int level, int stage)
        {
            if (m_levelDatabase != null)
            {
                return m_levelDatabase.GetStageData(level, stage);
            }
            
            return null;
        }
        
        public void CompleteStage()
        {
            Debug.Log($"[LevelManager] Stage {CurrentStage} of Level {CurrentLevel} completed.");
            
            OnStageCompleted?.Invoke(CurrentLevel, CurrentStage);
            
            int totalStages = GetStagesForLevel(CurrentLevel);
            
            if (CurrentStage < totalStages)
            {
                SetStage(CurrentStage + 1);
            }
            else
            {
                CompleteLevel();
            }
        }
        
        public void CompleteLevel()
        {
            Debug.Log($"[LevelManager] Level {CurrentLevel} completed!");
            
            OnLevelCompleted?.Invoke(CurrentLevel);
            
            if (CurrentLevel < GetTotalLevels())
            {
                SetLevel(CurrentLevel + 1);
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
            
            if (CurrentLevel != level)
            {
                CurrentLevel = level;
                CurrentStage = 1;
                
                Debug.Log($"[LevelManager] Level set to {CurrentLevel}");
                
                OnLevelChanged?.Invoke(CurrentLevel);
                OnStageChanged?.Invoke(CurrentLevel, CurrentStage);
                
                // Fire config loaded events if using database
                if (m_levelDatabase != null)
                {
                    LevelConfig levelConfig = GetCurrentLevelConfig();
                    if (levelConfig != null)
                    {
                        OnLevelConfigLoaded?.Invoke(levelConfig);
                    }
                    
                    StageData stageData = GetCurrentStageData();
                    if (stageData != null)
                    {
                        OnStageDataLoaded?.Invoke(stageData);
                    }
                }
                
                SaveProgress();
            }
        }
        
        public void SetStage(int stage)
        {
            int totalStages = GetStagesForLevel(CurrentLevel);
            
            if (stage < 1 || stage > totalStages)
            {
                Debug.LogError($"[LevelManager] Cannot set stage to {stage}. Valid range for level {CurrentLevel}: 1-{totalStages}");
                return;
            }
            
            if (CurrentStage != stage)
            {
                CurrentStage = stage;
                
                Debug.Log($"[LevelManager] Stage set to {CurrentStage} in Level {CurrentLevel}");
                
                OnStageChanged?.Invoke(CurrentLevel, CurrentStage);
                
                if (m_levelDatabase != null)
                {
                    StageData stageData = GetCurrentStageData();
                    if (stageData != null)
                    {
                        OnStageDataLoaded?.Invoke(stageData);
                    }
                }
                
                SaveProgress();
            }
        }
        
        public void ResetProgress()
        {
            Debug.Log("[LevelManager] Resetting progress to Level 1, Stage 1");
            
            CurrentLevel = 1;
            CurrentStage = 1;
            
            OnLevelChanged?.Invoke(CurrentLevel);
            OnStageChanged?.Invoke(CurrentLevel, CurrentStage);
            
            SaveProgress();
        }
        
        public void RestartStage()
        {
            Debug.Log($"[LevelManager] Restarting Stage {CurrentStage} of Level {CurrentLevel}");
            OnStageChanged?.Invoke(CurrentLevel, CurrentStage);
        }
        
        public void RestartLevel()
        {
            Debug.Log($"[LevelManager] Restarting Level {CurrentLevel}");
            SetStage(1);
        }
        
        public bool IsLastStageOfLevel()
        {
            return CurrentStage == GetStagesForLevel(CurrentLevel);
        }
        
        public bool IsLastLevel()
        {
            return CurrentLevel == GetTotalLevels();
        }
        
        private void SaveProgress()
        {
            if (m_progressStorage != null)
            {
                m_progressStorage.SaveProgress(CurrentLevel, CurrentStage);
            }
        }
        
        private void LoadProgress()
        {
            if (m_progressStorage != null)
            {
                var (level, stage) = m_progressStorage.LoadProgress(1, 1);
                CurrentLevel = level;
                CurrentStage = stage;
                Debug.Log($"[LevelManager] Progress loaded from storage: Level {CurrentLevel}, Stage {CurrentStage}");
            }
            else
            {
                CurrentLevel = 1;
                CurrentStage = 1;
                Debug.Log($"[LevelManager] No storage enabled, starting at Level 1, Stage 1");
            }
        }
    }
}