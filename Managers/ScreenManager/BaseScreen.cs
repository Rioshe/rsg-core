using System;
using UnityEngine;

namespace RSG
{
    public abstract class BaseScreen : MonoBehaviour
    {
        [SerializeField] private bool m_allowBackButton;
        private Action m_onBackPressed;
        public void SetBackPressCallback(Action onBackPressed)
        {
            m_onBackPressed = onBackPressed;
        }
        
        public virtual void Update()
        {
            if( m_allowBackButton )
            {
                if (UnityEngine.InputSystem.Keyboard.current.escapeKey.isPressed)
                {
                    m_onBackPressed?.Invoke();
                }
            }
        }
    }
}