using System.Threading.Tasks;
using UnityEngine;

namespace RSG
{
    public class SplashSystem : BootSystemBase
    {
        [Header("UI Configuration")]
        [SerializeField] private CanvasGroup m_splashGroup;
        [SerializeField] private float m_fadeDuration = 1.0f;

        [Header("Timing Configuration")]
        [SerializeField] private float m_minDisplayDuration = 2.0f;

        public override void Initialize()
        {
            if (!m_splashGroup) return;
            m_splashGroup.alpha = 1f;
            m_splashGroup.blocksRaycasts = true;
        }

        public async Task PlaySplashSequenceAsync()
        {
            await Task.Delay( (int) (m_minDisplayDuration * 1000) );
        }

        public async Task FadeOutAsync()
        {
            if (!m_splashGroup) return;

            await FadeAsync(m_splashGroup.alpha, 0f);

            m_splashGroup.alpha = 0f;
            m_splashGroup.blocksRaycasts = false;
        }

        private async Task FadeAsync(float from, float to)
        {
            float time = 0;
            while (time < m_fadeDuration)
            {
                time += Time.deltaTime;
                m_splashGroup.alpha = Mathf.Lerp(from, to, time / m_fadeDuration);
                await Task.Yield();
            }
        }
    }
}