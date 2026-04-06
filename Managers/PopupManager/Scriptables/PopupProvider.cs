using System;
using System.Collections.Generic;
using UnityEngine;

namespace RSG
{
    [CreateAssetMenu(fileName = "PopupProvider", menuName = "RSG/PopupManager/Popup Provider")]
    public class PopupProvider : ScriptableObject
    {
        [SerializeField] private List<BasePopup> m_popupPrefabs = new List<BasePopup>();
        private Dictionary<Type, BasePopup> m_popupDictionary;

        public T GetPopupPrefab<T>() where T : BasePopup
        {
            if (m_popupDictionary == null)
            {
                Initialize();
            }

            if (m_popupDictionary != null && m_popupDictionary.TryGetValue(typeof(T), out BasePopup popup))
            {
                return popup as T;
            }

            Debug.LogError($"[PopupProvider] Popup Type {typeof(T).Name} not found in provider list!");
            return null;
        }

        private void Initialize()
        {
            m_popupDictionary = new Dictionary<Type, BasePopup>();
            foreach (BasePopup prefab in m_popupPrefabs)
            {
                if (!prefab) continue;

                Type type = prefab.GetType();

                if (!m_popupDictionary.TryAdd(type, prefab))
                {
                    Debug.LogError($"[PopupProvider] Duplicate Type {type.Name} detected.");
                }
            }
        }
    }
}

