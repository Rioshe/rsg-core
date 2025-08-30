using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace RSG.Pool
{
    [Serializable]
    public class PoolService<TMonoBehaviour, TEnum>
        where TMonoBehaviour : MonoBehaviour 
        where TEnum : Enum
    {
        private readonly PoolDatabase<TMonoBehaviour, TEnum> m_database;
        private readonly Transform m_parent;
        private readonly int m_defaultCapacity;
        private readonly int m_maxCapacity;

        private Dictionary<TEnum, ObjectPool<TMonoBehaviour>> m_pools = new Dictionary<TEnum, ObjectPool<TMonoBehaviour>>();

        public PoolService(PoolDatabase<TMonoBehaviour, TEnum> mDatabase, Transform mParent = null, int mDefaultCapacity = 10, int mMaxCapacity = 200)
        {
            m_database = mDatabase;
            m_parent = mParent;
            m_defaultCapacity = mDefaultCapacity;
            m_maxCapacity = mMaxCapacity;

            InitialisePools();
        }

        #region Public API

        public TMonoBehaviour Spawn(TEnum type)
        {
            return GetFromPool(type);
        }

        public TMonoBehaviour Spawn(TEnum type, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            TMonoBehaviour instance = GetFromPool(type);
            if (instance)
            {
                instance.transform.SetParent(parent ?? m_parent, false);
                instance.transform.SetPositionAndRotation(position, rotation);
            }
            return instance;
        }

        public TMonoBehaviour Spawn(TEnum type, Vector3 position) => Spawn(type, position, Quaternion.identity);

        private TMonoBehaviour GetFromPool(TEnum type)
        {
            if (!m_pools.TryGetValue(type, out ObjectPool<TMonoBehaviour> pool))
            {
                Debug.LogError($"No pool for {typeof(TMonoBehaviour)} type {type}");
                return null;
            }
            return pool.Get();
        }
        
        public void Despawn(TMonoBehaviour instance)
        {
            if (instance is not IPoolable<TMonoBehaviour, TEnum> poolable)
            {
                Debug.LogError($"{instance.name} is not poolable!");
                UnityEngine.Object.Destroy(instance.gameObject);
                return;
            }

            if (!m_pools.TryGetValue(poolable.PoolType, out ObjectPool<TMonoBehaviour> pool))
            {
                Debug.LogError($"No pool for {typeof(TMonoBehaviour)} type {poolable.PoolType}");
                UnityEngine.Object.Destroy(instance.gameObject);
                return;
            }

            pool.Release(instance);
        }

        
        public IEnumerable<TMonoBehaviour> GetActiveInstances(TEnum type)
        {
            if (!m_pools.TryGetValue(type, out ObjectPool<TMonoBehaviour> pool))
                yield break;

            // Unity's ObjectPool does not track active instances,
            // but you can extend this yourself (see below).
            // For now, this is just a placeholder for when you want to expand.
        }

        public void Prewarm(TEnum type, int count)
        {
            if (!m_pools.TryGetValue(type, out ObjectPool<TMonoBehaviour> pool))
                return;

            List<TMonoBehaviour> temp = new List<TMonoBehaviour>(count);
            for (int i = 0; i < count; i++)
                temp.Add(pool.Get());

            foreach (TMonoBehaviour obj in temp)
                pool.Release(obj);
        }

        public void Clear(TEnum type)
        {
            if (!m_pools.TryGetValue(type, out ObjectPool<TMonoBehaviour> pool))
                return;

            pool.Clear();
        }

        public void ClearAll()
        {
            foreach (ObjectPool<TMonoBehaviour> pool in m_pools.Values)
                pool.Clear();
        }

        #endregion

        private void InitialisePools()
        {
            m_pools.Clear();
            foreach (TEnum type in Enum.GetValues(typeof(TEnum)))
            {
                ObjectPool<TMonoBehaviour> pool = new ObjectPool<TMonoBehaviour>(
                    createFunc: () => Create(type),
                    actionOnGet: OnGet,
                    actionOnRelease: OnRelease,
                    actionOnDestroy: OnDestroy,
                    collectionCheck: false,
                    defaultCapacity: m_defaultCapacity,
                    maxSize: m_maxCapacity
                );
                m_pools[type] = pool;
            }
        }

        private TMonoBehaviour Create(TEnum type)
        {
            TMonoBehaviour prefab = m_database.GetPrefab(type);
            if (!prefab)
            {
                Debug.LogError($"No prefab found for {typeof(TMonoBehaviour)} type {type}");
                return null;
            }

            TMonoBehaviour instance = UnityEngine.Object.Instantiate(prefab, m_parent);

            if (instance is IPoolable<TMonoBehaviour, TEnum> poolable)
            {
                poolable.PoolService = this;
                poolable.PoolType = type; // inject the type too
            }

            OnCreated(instance, type);
            instance.gameObject.SetActive(false);
            return instance;
        }



        protected virtual void OnCreated(TMonoBehaviour instance, TEnum type) { }
        protected virtual void OnGet(TMonoBehaviour instance) => instance.gameObject.SetActive(true);
        protected virtual void OnRelease(TMonoBehaviour instance) => instance.gameObject.SetActive(false);
        protected virtual void OnDestroy(TMonoBehaviour instance) => UnityEngine.Object.Destroy(instance.gameObject);
    }
}
