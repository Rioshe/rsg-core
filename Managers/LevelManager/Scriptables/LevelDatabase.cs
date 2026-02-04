using System;
using UnityEngine;

namespace RSG
{
    [CreateAssetMenu(fileName = "LevelDatabase", menuName = "RSG/LevelManager/Level Database", order = 1)]
    public class LevelDatabase : ScriptableObject
    {
        [SerializeField] private LevelConfig[] m_levels = Array.Empty<LevelConfig>();
        
        public LevelConfig[] Levels
        {
            get => m_levels;
        }

        public int TotalLevels
        {
            get => m_levels.Length;
        }

        public LevelConfig GetLevel(int levelNumber)
        {
            if (levelNumber < 1 || levelNumber > m_levels.Length)
            {
                Debug.LogError($"[LevelDatabase] Level {levelNumber} does not exist. Total levels: {m_levels.Length}");
                return null;
            }
            
            return m_levels[levelNumber - 1];
        }
        
        public int GetStagesForLevel(int levelNumber)
        {
            LevelConfig level = GetLevel(levelNumber);
            return level ? level.TotalStages : 0;
        }
        
        public StageData GetStageData(int levelNumber, int stageNumber)
        {
            LevelConfig level = GetLevel(levelNumber);
            if (!level) return null;
            
            return level.GetStage(stageNumber - 1);
        }
    }
}

