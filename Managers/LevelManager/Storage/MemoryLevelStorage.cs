namespace RSG
{
    public class MemoryLevelStorage : ILevelProgressStorage
    {
        private int m_savedLevel = 1;
        private int m_savedStage = 1;
        
        public void SaveProgress(int level, int stage)
        {
            m_savedLevel = level;
            m_savedStage = stage;
        }
        
        public (int level, int stage) LoadProgress(int defaultLevel = 1, int defaultStage = 1)
        {
            // Return saved values if they exist, otherwise return defaults
            return m_savedLevel > 0 ? (m_savedLevel, m_savedStage) : (defaultLevel, defaultStage);
        }
    }
}

