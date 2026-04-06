using System;
using System.Collections.Generic;
using UnityEngine;

namespace RSG
{
    public class PopupManager : MonoSingleton<PopupManager>
    {
        [SerializeField] private PopupProvider m_popupProvider;

        private Canvas m_canvas;
        private readonly Stack<BasePopup> m_popupStack = new Stack<BasePopup>();
        private readonly Dictionary<Type, BasePopup> m_cachedPopups = new Dictionary<Type, BasePopup>();

        public bool HasActivePopup => m_popupStack.Count > 0;

        public void RegisterCanvas(Canvas canvas)
        {
            m_canvas = canvas;
        }

        public T ShowPopup<T>() where T : BasePopup
        {
            Type popupType = typeof(T);

            if (m_popupStack.Count > 0 && m_popupStack.Peek().GetType() == popupType)
            {
                return m_popupStack.Peek() as T;
            }

            if (!m_cachedPopups.TryGetValue(popupType, out BasePopup instance))
            {
                BasePopup prefab = m_popupProvider.GetPopupPrefab<T>();
                if (!prefab) return null;

                instance = Instantiate(prefab, m_canvas.transform);
                instance.gameObject.SetActive(false);
                m_cachedPopups.Add(popupType, instance);
            }

            instance.transform.SetAsLastSibling();
            instance.gameObject.SetActive(true);
            instance.SetCloseCallback(HideCurrentPopup);

            m_popupStack.Push(instance);

            return instance as T;
        }

        public void HideCurrentPopup()
        {
            if (m_popupStack.Count == 0) return;

            BasePopup current = m_popupStack.Pop();
            current.SetCloseCallback(null);
            current.gameObject.SetActive(false);

            if (m_popupStack.Count > 0)
            {
                BasePopup previous = m_popupStack.Peek();
                previous.SetCloseCallback(HideCurrentPopup);
            }
        }

        public void HideAllPopups()
        {
            while (m_popupStack.Count > 0)
            {
                HideCurrentPopup();
            }
        }

        public void HidePopup<T>() where T : BasePopup
        {
            Type popupType = typeof(T);
            
            if (m_popupStack.Count == 0) return;
            
            if (m_popupStack.Peek().GetType() == popupType)
            {
                HideCurrentPopup();
            }
        }
    }
}