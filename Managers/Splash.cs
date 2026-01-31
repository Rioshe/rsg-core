using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RSG
{
    public class Splash : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private CanvasGroup m_splashGroup;
        [SerializeField] private float m_fadeDuration = 1.0f;
        [SerializeField] private float m_minDisplayDuration = 2.0f;

        private List<Task> m_externalLoadingTasks = new List<Task>();

        private void Awake()
        {
            GameStateManager.OnStateChanged += HandleStateChanged;
            m_splashGroup.alpha = 0f; 
        }

        private void OnDestroy()
        {
            GameStateManager.OnStateChanged -= HandleStateChanged;
        }
        
        public void SetLoadDependency(Task loadingTask) => m_externalLoadingTasks.Add(loadingTask);

        private void HandleStateChanged(GameState state)
        {
            if (state == GameState.Splash)
            {
                StartCoroutine(SplashSequence());
            }
        }

        private IEnumerator SplashSequence()
        {
            m_splashGroup.alpha = 1f;
            m_splashGroup.blocksRaycasts = true;

            float timer = 0;
            while(timer < m_minDisplayDuration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            if (m_externalLoadingTasks.Count > 0)
            {
                Task taskGroup = Task.WhenAll(m_externalLoadingTasks);
                yield return new WaitUntil(() => taskGroup.IsCompleted);
            }

            yield return StartCoroutine(FadeRoutine(1f, 0f));

            m_splashGroup.blocksRaycasts = false;
            m_externalLoadingTasks.Clear();

            GameStateManager.SetState(GameState.LoadMainScene);
        }

        private IEnumerator FadeRoutine(float from, float to)
        {
            float elapsed = 0f;
            while (elapsed < m_fadeDuration)
            {
                elapsed += Time.deltaTime;
                m_splashGroup.alpha = Mathf.Lerp(from, to, elapsed / m_fadeDuration);
                yield return null;
            }
            m_splashGroup.alpha = to;
        }
    }
}