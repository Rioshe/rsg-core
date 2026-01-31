using System.Collections.Generic;
using UnityEngine;

namespace RSG
{
    public class ScreenManager : MonoSingleton<ScreenManager>
    {
        [SerializeField] private Canvas m_canvas;
        [SerializeField] private ScreenProvider m_screenProvider;
        
        private readonly Stack<BaseScreen> m_screenStack = new Stack<BaseScreen>();
        private readonly Dictionary<int, BaseScreen> m_cachedScreens = new Dictionary<int, BaseScreen>();

        protected override void Init()
        {
            CoreUIEvents.OnRequestShowScreen += CoreUIEvent_OnRequestShowScreen;
            CoreUIEvents.OnRequestHideCurrent += CoreUIEvent_OnRequestHideCurrent;
        }

        private void OnDestroy()
        {
            CoreUIEvents.OnRequestShowScreen -= CoreUIEvent_OnRequestShowScreen;
            CoreUIEvents.OnRequestHideCurrent -= CoreUIEvent_OnRequestHideCurrent;
        }
        
        private void CoreUIEvent_OnRequestShowScreen( int id )
        {
            ShowScreen<BaseScreen>( id );
        }

        private void CoreUIEvent_OnRequestHideCurrent()
        {
            HideCurrentScreen();
        }

        public T ShowScreen<T>(int screenId) where T : BaseScreen
        {
            if (m_screenStack.Count > 0 && m_screenStack.Peek().GetId() == screenId)
            {
                return m_screenStack.Peek() as T;
            }

            if (!m_cachedScreens.TryGetValue(screenId, out BaseScreen instance))
            {
                BaseScreen prefab = m_screenProvider.GetScreenPrefab(screenId);
                if (prefab == null) return null;

                instance = Instantiate(prefab, m_canvas.transform);
                instance.gameObject.SetActive(false); 
                m_cachedScreens.Add(screenId, instance);
            }
            
            if (instance is not T typedInstance)
            {
                Debug.LogError($"[ScreenManager] ID {screenId} is not type {typeof(T).Name}");
                return null;
            }

            if (m_screenStack.Count > 0)
            {
                BaseScreen top = m_screenStack.Peek();
                top.OnScreenHide();
                top.gameObject.SetActive(false);
            }

            instance.transform.SetAsLastSibling();
            instance.gameObject.SetActive(true);
            instance.OnScreenShow();
            
            m_screenStack.Push(instance);
            
            CoreUIEvents.SendScreenShow(screenId); 

            return typedInstance;
        }

        public void HideCurrentScreen()
        {
            if (m_screenStack.Count == 0) return;

            BaseScreen current = m_screenStack.Pop();
            current.OnScreenHide();
            current.gameObject.SetActive(false);
            CoreUIEvents.SendScreenHide(current.GetId());

            if (m_screenStack.Count > 0)
            {
                BaseScreen previous = m_screenStack.Peek();
                previous.gameObject.SetActive(true);
                previous.OnScreenShow();
            }
        }
    }
}