using System.Threading.Tasks;
using UnityEngine;

namespace RSG
{
    public class TransitionService : MonoBehaviour, ITransitionService
    {
        [Header("Configuration")]
        [SerializeField] private float m_fadeDuration = 0.5f;

        [Header("UI Overlays")]
        [SerializeField] private CanvasGroup m_loadingOverlay;

        
        public Task InitializeAsync()
        {
            if (!m_loadingOverlay) return Task.CompletedTask;
            m_loadingOverlay.alpha = 0f;
            m_loadingOverlay.blocksRaycasts = false;
            ServiceLocator.Register<ITransitionService>(this);
            return Task.CompletedTask;
        }
        public Task ShutdownAsync()
        {
            ServiceLocator.Unregister<ITransitionService>(this);
            return Task.CompletedTask;
        }
        
        public async Task ShowLoadingAsync()
        {
            if (m_loadingOverlay)
                await FadeAsync(m_loadingOverlay, 1f, m_fadeDuration);
        }

        public async Task HideLoadingAsync()
        {
            if (m_loadingOverlay)
                await FadeAsync(m_loadingOverlay, 0f, m_fadeDuration);
        }

        private async Task FadeAsync(CanvasGroup group, float targetAlpha, float duration)
        {
            if (targetAlpha > 0) group.blocksRaycasts = true;

            float startAlpha = group.alpha;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                group.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                await Task.Yield();
            }

            group.alpha = targetAlpha;
            if (targetAlpha <= 0) group.blocksRaycasts = false;
        }
    }
}