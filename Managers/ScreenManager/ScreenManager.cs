using System;
using System.Collections.Generic;
using UnityEngine;

namespace RSG
{
    public class ScreenManager : MonoSingleton<ScreenManager>
    {
        [SerializeField] private Canvas m_canvas;
        [SerializeField] private ScreenProvider m_screenProvider;
        
        private readonly Stack<BaseScreen> m_screenStack = new Stack<BaseScreen>();
        private readonly Dictionary<Type, BaseScreen> m_cachedScreens = new Dictionary<Type, BaseScreen>();

        public T ShowScreen<T>() where T : BaseScreen
        {
            Type screenType = typeof(T);

            if (m_screenStack.Count > 0 && m_screenStack.Peek().GetType() == screenType)
            {
                return m_screenStack.Peek() as T;
            }

            if (!m_cachedScreens.TryGetValue(screenType, out BaseScreen instance))
            {
                BaseScreen prefab = m_screenProvider.GetScreenPrefab<T>();
                if (!prefab) return null;

                instance = Instantiate(prefab, m_canvas.transform);
                instance.gameObject.SetActive(false); 
                m_cachedScreens.Add(screenType, instance);
            }
            
            if (m_screenStack.Count > 0)
            {
                BaseScreen top = m_screenStack.Peek();
                top.gameObject.SetActive(false);
            }

            instance.transform.SetAsLastSibling();
            instance.gameObject.SetActive(true);
            instance.SetBackPressCallback(HideCurrentScreen);
            
            m_screenStack.Push(instance);
            
            return instance as T;
        }

        public void HideCurrentScreen()
        {
            if (m_screenStack.Count == 0) return;

            BaseScreen current = m_screenStack.Pop();
            current.SetBackPressCallback(null);
            current.gameObject.SetActive(false);

            if (m_screenStack.Count > 0)
            {
                BaseScreen previous = m_screenStack.Peek();
                previous.gameObject.SetActive(true);
                previous.SetBackPressCallback(HideCurrentScreen);
            }
        }
    }
}