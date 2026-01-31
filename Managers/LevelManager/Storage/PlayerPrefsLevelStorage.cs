namespace RSG
{
    public class PlayerPrefsLevelStorage : ILevelProgressStorage
    {
        public void SaveProgress(int level, int stage)
        {
            PrefsManager.SetInt(PrefsBucket.INT_CURRENT_LEVEL, level);
            PrefsManager.SetInt(PrefsBucket.INT_CURRENT_STAGE, stage);
            PrefsManager.Save();
        }
        
        public (int level, int stage) LoadProgress(int defaultLevel = 1, int defaultStage = 1)
        {
            int level = PrefsManager.GetInt(PrefsBucket.INT_CURRENT_LEVEL, defaultLevel);
            int stage = PrefsManager.GetInt(PrefsBucket.INT_CURRENT_STAGE, defaultStage);
            return (level, stage);
        }
    }
}

