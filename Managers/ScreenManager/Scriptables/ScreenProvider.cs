using System;
using System.Collections.Generic;
using UnityEngine;

namespace RSG
{
    [CreateAssetMenu(fileName = "ScreenProvider", menuName = "RSG/ScreenManager/Screen Provider")]
    public class ScreenProvider : ScriptableObject
    {
        [SerializeField] private List<BaseScreen> m_screenPrefabs = new List<BaseScreen>();
        private Dictionary<Type, BaseScreen> m_screenDictionary;

        public T GetScreenPrefab<T>() where T : BaseScreen
        {
            if (m_screenDictionary == null)
            {
                Initialize();
            }

            if (m_screenDictionary != null && m_screenDictionary.TryGetValue(typeof(T), out BaseScreen screen))
            {
                return screen as T;
            }
            
            Debug.LogError($"[ScreenProvider] Screen Type {typeof(T).Name} not found in provider list!");
            return null;
        }

        private void Initialize()
        {
            m_screenDictionary = new Dictionary<Type, BaseScreen>();
            foreach (BaseScreen prefab in m_screenPrefabs)
            {
                if (!prefab) continue;
                
                Type type = prefab.GetType();

                if (!m_screenDictionary.TryAdd( type, prefab ))
                {
                    Debug.LogError($"[ScreenProvider] Duplicate Type {type.Name} detected.");
                }
            }
        }
    }
}