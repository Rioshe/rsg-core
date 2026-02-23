using UnityEngine;

namespace RSG
{
    [CreateAssetMenu(fileName = "LevelDatabase", menuName = "RSG/LevelManager/Level Database")]
    public class LevelDatabase : ScriptableObject
    {
        [SerializeField] private LevelConfigBase[] m_levels;
        
        public int TotalLevels
        {
            get => m_levels.Length;
        }

        public LevelConfigBase GetLevel(int index)
        {
            if (index < 0 || index >= m_levels.Length) return null;
            return m_levels[index];
        }
    }
}