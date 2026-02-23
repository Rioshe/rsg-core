using UnityEngine;

namespace RSG
{
    public class ProgressManager : MonoSingleton<ProgressManager>
    {
        public int SavedLevel { get; private set; }
        public bool IsTutorialCompleted { get; private set; }

        protected override void Init()
        {
            LoadProgress();
        }

        private void LoadProgress()
        {
            SavedLevel = PrefsManager.GetInt(PrefKeys.INT_CURRENT_LEVEL, 1);
            IsTutorialCompleted = PrefsManager.GetBool(PrefKeys.BOOL_IS_TUTORIAL_COMPLETED, false);
            
            Debug.Log($"[ProgressManager] Loaded: Level {SavedLevel}, Tutorial {IsTutorialCompleted}");
        }

        public void SaveProgress(int level, int stage)
        {
            SavedLevel = level;

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

        public void ResetProgress()
        {
            SavedLevel = 1;
            IsTutorialCompleted = false;

            // Option A: Delete specific keys (Safer if you have settings mixed in)
            PrefsManager.DeleteKey(PrefKeys.INT_CURRENT_LEVEL);
            PrefsManager.DeleteKey(PrefKeys.BOOL_IS_TUTORIAL_COMPLETED);

            // Option B: Delete All (Nuclear option)
            // PrefsManager.DeleteAll(); 

            PrefsManager.Save();
            Debug.Log("[ProgressManager] Progress Reset.");
        }
    }
}