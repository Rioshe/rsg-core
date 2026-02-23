using UnityEngine;

namespace RSG
{
    public abstract class LevelConfigBase : ScriptableObject
    {
        [Header("Core Level Info")]
        [SerializeField] private string m_levelName = "Level";
        [SerializeField, TextArea] private string m_levelDescription;

        public string LevelName
        {
            get => m_levelName;
        }

        public string Description
        {
            get => m_levelDescription;
        }
    }
}