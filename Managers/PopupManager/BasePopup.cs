using System;
using UnityEngine;

namespace RSG
{
    public abstract class BasePopup : MonoBehaviour
    {
        [SerializeField] private bool m_allowBackButton = true;
        [SerializeField] private bool m_closeOnBackgroundClick = true;
        
        private Action m_onCloseCallback;

        public bool CloseOnBackgroundClick => m_closeOnBackgroundClick;

        public void SetCloseCallback(Action onClose)
        {
            m_onCloseCallback = onClose;
        }

        public virtual void Update()
        {
            if (m_allowBackButton)
            {
                if (UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    Close();
                }
            }
        }

        public virtual void Close()
        {
            m_onCloseCallback?.Invoke();
        }

        public virtual void OnShow()
        {
            // Override in derived classes for custom show logic
        }

        public virtual void OnHide()
        {
            // Override in derived classes for custom hide logic
        }
    }
}

