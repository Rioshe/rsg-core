using UnityEngine;

namespace RSG
{
    public abstract class BaseScreen : MonoBehaviour
    {
        [SerializeField] private bool m_allowBackButton = true;

        private bool m_isSubscribed;

        protected virtual void OnEnable()
        {
            Subscribe();
        }

        protected virtual void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            if (m_isSubscribed) return;
            CoreUIEvents.OnBackButtonPressed += HandleBackPress;
            m_isSubscribed = true;
        }

        private void Unsubscribe()
        {
            if (!m_isSubscribed) return;
            CoreUIEvents.OnBackButtonPressed -= HandleBackPress;
            m_isSubscribed = false;
        }
        
        private void HandleBackPress()
        {
            if (m_allowBackButton)
            {
                CoreUIEvents.RequestHideCurrent();
            }
        }
        
        public abstract int GetId();
        public virtual void OnScreenShow() { }
        public virtual void OnScreenHide() { }
    }
}