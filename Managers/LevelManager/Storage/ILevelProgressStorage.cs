namespace RSG
{
    public interface ILevelProgressStorage
    {
        void SaveProgress(int level, int stage);
        (int level, int stage) LoadProgress(int defaultLevel = 1, int defaultStage = 1);
    }
}

