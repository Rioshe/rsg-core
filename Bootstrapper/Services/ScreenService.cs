using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RSG
{
    public class ScreenService : MonoBehaviour, IScreenService
    {
        [Header("Configuration")]
        [SerializeField] private Transform m_root;
    
        private readonly Dictionary<string, ScreenBase> m_screens = new();
        private string m_currentId;
        
        public Task InitializeAsync()
        {
            DiscoverScreens();
            // Start with all hidden; do not invoke channel on init to avoid side effects
            HideAll();
            
            ServiceLocator.Register<IScreenService>(this);
            return Task.CompletedTask;
            // ScreenEvents.OnShowScreen += ScreenEvent_ShowById;
        }
        public Task ShutdownAsync()
        {
            ServiceLocator.Unregister<IScreenService>(this);
            return Task.CompletedTask;
            //ScreenEvents.OnShowScreen -= ScreenEvent_ShowById;
        }

        private void DiscoverScreens()
        {
            ScreenBase[] found = (m_root ? m_root : transform).GetComponentsInChildren<ScreenBase>(true);
            m_screens.Clear();
            foreach (ScreenBase screen in found)
            {
                if (!screen) continue;
                string id = screen.ScreenId;
                if (string.IsNullOrEmpty(id)) continue;
                m_screens[id] = screen;
            }
        }


        public void ScreenEvent_ShowById(string id)
        {
            // No-op if already showing
            if (!string.IsNullOrEmpty(m_currentId) && m_currentId == id)
                return;
            
            if (string.IsNullOrEmpty(id) || !m_screens.TryGetValue(id, out var target))
            {
                Debug.LogWarning($"ScreenSystem: Unknown or empty screen id '{id}'.");
                return;
            }

            // Hide previously shown screen only, not all
            if (!string.IsNullOrEmpty(m_currentId) && m_screens.TryGetValue(m_currentId, out var current))
            {
                current.OnHide();
            }

            target.OnShow();
            m_currentId = id;
        }

        public void HideAll()
        {
            foreach (ScreenBase screen in m_screens.Values)
            {
                screen.OnHide();
            }
            m_currentId = null;
        }

    }
}