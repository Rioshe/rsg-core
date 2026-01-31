using System.Collections.Generic;
using UnityEngine;

namespace RSG
{
    [CreateAssetMenu(fileName = "ScreenProvider", menuName = "Screen/Screen Provider")]
    public class ScreenProvider : ScriptableObject
    {
        [SerializeField] private List<BaseScreen> m_screenPrefabs = new List<BaseScreen>();
        private Dictionary<int, BaseScreen> m_screenDictionary;

        public BaseScreen GetScreenPrefab(int screenId)
        {
            if (m_screenDictionary == null)
            {
                Initialize();
            }

            if (m_screenDictionary != null && m_screenDictionary.TryGetValue(screenId, out BaseScreen screen))
            {
                return screen;
            }
            
            Debug.LogError($"[ScreenProvider] Screen ID {screenId} not found!");
            return null;
        }

        private void Initialize()
        {
            m_screenDictionary = new Dictionary<int, BaseScreen>();
            foreach (BaseScreen prefab in m_screenPrefabs)
            {
                if (!prefab) continue;
                if (m_screenDictionary.ContainsKey(prefab.GetId()))
                {
                    Debug.LogError($"[ScreenProvider] Duplicate ID {prefab.GetId()} detected.");
                    continue;
                }
                m_screenDictionary.Add(prefab.GetId(), prefab);
            }
        }
    }
}