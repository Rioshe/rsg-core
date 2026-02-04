using UnityEngine;

namespace RSG
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "RSG/LevelManager/Level Config", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        [Header("Level Info")]
        [SerializeField] private int m_levelNumber = 1;
        [SerializeField] private string m_levelName = "Level 1";
        [SerializeField, TextArea(2, 4)] private string m_levelDescription = "";
        
        [Header("Stage Configuration")]
        [SerializeField] private int m_totalStages = 5;
        [SerializeField] private StageData[] m_stages = new StageData[5];
        
        public int LevelNumber
        {
            get => m_levelNumber;
        }

        public string LevelName
        {
            get => m_levelName;
        }

        public string LevelDescription
        {
            get => m_levelDescription;
        }

        public int TotalStages
        {
            get => m_totalStages;
        }

        public StageData[] Stages
        {
            get => m_stages;
        }

        public StageData GetStage(int stageIndex)
        {
            if (stageIndex < 0 || stageIndex >= m_stages.Length)
            {
                Debug.LogWarning($"[LevelConfig] Stage index {stageIndex} out of range for {m_levelName}");
                return null;
            }
            return m_stages[stageIndex];
        }
    }
    
    [System.Serializable]
    public class StageData
    {
        public int stageNumber = 1;
        public string stageName = "Stage 1";
        [TextArea(2, 3)] public string stageDescription = "";
        public float targetTime = 60f;
        public int targetScore = 1000;
    }
}

