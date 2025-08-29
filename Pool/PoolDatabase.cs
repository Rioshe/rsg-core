using System;
using System.Collections.Generic;
using UnityEngine;

namespace RSG.Pool
{
    [Serializable]
    internal struct PoolEntry<TPrefab, TEnum> where TPrefab : MonoBehaviour where TEnum : Enum
    {
        public TEnum type;
        public TPrefab prefab;
    }

    //[CreateAssetMenu(fileName = "PoolDatabase", menuName = "PoolDatabase/PoolDatabase")]
    public abstract class PoolDatabase<TPrefab, TEnum> : ScriptableObject 
        where TPrefab : MonoBehaviour where TEnum : Enum
    {
        [SerializeField] private List<PoolEntry<TPrefab, TEnum>> entries;
        private Dictionary<TEnum, TPrefab> m_lookup;

        public TPrefab GetPrefab(TEnum type)
        {
            if (m_lookup == null)
            {
                m_lookup = new Dictionary<TEnum, TPrefab>();
                foreach (PoolEntry<TPrefab, TEnum> entry in entries)
                    m_lookup[entry.type] = entry.prefab;
            }

            return m_lookup.TryGetValue(type, out TPrefab prefab) ? prefab : null;
        }
    }
}