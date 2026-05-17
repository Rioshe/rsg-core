using UnityEngine;

namespace RSG
{
    [RequireComponent(typeof(Canvas))]
    public abstract class CanvasBase : MonoBehaviour
    {
        [SerializeField] private RenderMode m_renderMode = RenderMode.ScreenSpaceCamera;

        protected Canvas Canvas { get; private set; }

        protected virtual void Awake()
        {
            Canvas = GetComponent<Canvas>();
            Canvas.renderMode = m_renderMode;
            if (m_renderMode == RenderMode.ScreenSpaceCamera)
                Canvas.worldCamera = Camera.main;
            Canvas.sortingLayerName = SortingLayerName;
        }

        protected abstract string SortingLayerName { get; }
    }
}

