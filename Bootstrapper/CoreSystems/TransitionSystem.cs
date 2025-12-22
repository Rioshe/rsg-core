using System.Threading.Tasks;
using UnityEngine;

namespace RSG
{
    public class TransitionSystem : BootSystemBase
    {
        [Header("Configuration")]
        [SerializeField] private float m_fadeDuration = 0.5f;

        [Header("UI Overlays")]
        [SerializeField] private CanvasGroup m_loadingOverlay;

        public override void Initialize()
        {
            if (!m_loadingOverlay) return;
            m_loadingOverlay.alpha = 0f;
            m_loadingOverlay.blocksRaycasts = false;
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