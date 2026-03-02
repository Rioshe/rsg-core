using UnityEngine;

namespace RSG
{
    public class ProgressManager : MonoSingleton<ProgressManager>
    {
        public int Level { get; private set; }
        public bool IsTutorialCompleted { get; private set; }

        protected override void Init()
        {
            LoadProgress();
        }

        private void LoadProgress()
        {
            Level = PrefsManager.GetInt(PrefKeys.INT_CURRENT_LEVEL, 1);
            IsTutorialCompleted = PrefsManager.GetBool(PrefKeys.BOOL_IS_TUTORIAL_COMPLETED);
            
            Debug.Log($"[ProgressManager] Loaded: Level {Level}, Tutorial {IsTutorialCompleted}");
        }

        public void SaveProgress(int level, int stage)
        {
            Level = level;

            PrefsManager.SetInt(PrefKeys.INT_CURRENT_LEVEL, level);
            PrefsManager.Save();
            
            Debug.Log($"[ProgressManager] Progress Saved: Level {level}, Stage {stage}");
        }
        
        public void CompleteTutorial()
        {
            if (IsTutorialCompleted) return;

            IsTutorialCompleted = true;
            PrefsManager.SetBool(PrefKeys.BOOL_IS_TUTORIAL_COMPLETED, true);
            PrefsManager.Save();
            
            Debug.Log("[ProgressManager] Tutorial Completed!");
        }
    }
}