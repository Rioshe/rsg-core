using System;
using UnityEngine;

namespace RSG
{
    public abstract class BasePopup : MonoBehaviour
    {
        [SerializeField] private bool m_allowBackButton = true;
        
        private Action m_onCloseCallback;

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
    }
}

