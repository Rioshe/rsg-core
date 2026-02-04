namespace RSG
{
    public class SceneManager : MonoSingleton<SceneManager>
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
            UnityEngine.SceneManagement.SceneManager.LoadScene(MAIN_SCENE);
        }
    }
}