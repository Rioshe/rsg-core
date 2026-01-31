using UnityEngine.SceneManagement;

namespace RSG
{
    public class SceneLoader : MonoSingleton<SceneLoader>
    {
        private const string MAIN_SCENE = "_Main";

        protected override void Init()
        {
            GameStateManager.OnStateChanged += HandleStateChange;
        }

        private void OnDestroy()
        {
            GameStateManager.OnStateChanged -= HandleStateChange;
        }

        private void HandleStateChange(GameState state)
        {
            if(state == GameState.LoadMainScene)
            {
                LoadMainScene();
            }
        }
        
        private static void LoadMainScene()
        {
            SceneManager.LoadScene(MAIN_SCENE);
        }
    }
}