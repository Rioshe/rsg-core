using UnityEngine;

namespace RSG
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [Header("Views")]
        [SerializeField] private GameObject m_mainMenuView; // "Tap to Play"
        [SerializeField] private GameObject m_gameplayView; // HUD
        [SerializeField] private GameObject m_winView;
        [SerializeField] private GameObject m_failView;

        protected override void Init()
        {
            GameStateManager.OnStateChanged += HandleStateChanged;
        }

        private void OnDestroy()
        {
            GameStateManager.OnStateChanged -= HandleStateChanged;
        }

        private void HandleStateChanged(GameState state)
        {
            // Hide all first (Simple approach)
            m_mainMenuView.SetActive(false);
            m_gameplayView.SetActive(false);
            m_winView.SetActive(false);
            m_failView.SetActive(false);

            // Show specific view based on state
            switch (state)
            {
                case GameState.MainMenu:
                    m_mainMenuView.SetActive(true);
                    break;
                case GameState.Gameplay:
                    m_gameplayView.SetActive(true);
                    break;
                case GameState.Win:
                    m_winView.SetActive(true);
                    break;
                case GameState.Fail:
                    m_failView.SetActive(true);
                    break;
            }
        }
        
        // Linked to your "Tap to Play" button in the Inspector
        public void OnStartGameButtonClicked()
        {
            GameStateManager.SetState(GameState.Gameplay);
        }
    }
}