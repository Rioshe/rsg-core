using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace RSG.Pool
{
    [Serializable]
    public class PoolService<T, TType>
        where T : MonoBehaviour 
        where TType : Enum
    {
        private readonly PoolDatabase<T, TType> m_database;
        private readonly Transform m_parent;
        private readonly int m_defaultCapacity;
        private readonly int m_maxCapacity;

        private Dictionary<TType, ObjectPool<T>> m_pools = new Dictionary<TType, ObjectPool<T>>();

        public PoolService(PoolDatabase<T, TType> mDatabase, Transform mParent = null, int mDefaultCapacity = 10, int mMaxCapacity = 200)
        {
            m_database = mDatabase;
            m_parent = mParent;
            m_defaultCapacity = mDefaultCapacity;
            m_maxCapacity = mMaxCapacity;

            InitialisePools();
        }

        #region Public API

        public T Spawn(TType type)
        {
            return GetFromPool(type);
        }

        public T Spawn(TType type, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            T instance = GetFromPool(type);
            if (instance)
            {
                instance.transform.SetParent(parent ?? m_parent, false);
                instance.transform.SetPositionAndRotation(position, rotation);
            }
            return instance;
        }

        public T Spawn(TType type, Vector3 position) => Spawn(type, position, Quaternion.identity);

        private T GetFromPool(TType type)
        {
            if (!m_pools.TryGetValue(type, out ObjectPool<T> pool))
            {
                Debug.LogError($"No pool for {typeof(T)} type {type}");
                return null;
            }
            return pool.Get();
        }
        
        public void Despawn(TType type, T instance)
        {
            if (!m_pools.TryGetValue(type, out ObjectPool<T> pool))
            {
                Debug.LogError($"No pool for {typeof(T)} type {type}");
                UnityEngine.Object.Destroy(instance.gameObject); // fallback
                return;
            }
            pool.Release(instance);
        }
        
        /// <summary>
        /// Returns all currently active instances of a given type.
        /// (May be useful for debugging or gameplay logic like clearing bullets.)
        /// </summary>
        public IEnumerable<T> GetActiveInstances(TType type)
        {
            if (!m_pools.TryGetValue(type, out ObjectPool<T> pool))
                yield break;

            // Unity's ObjectPool does not track active instances,
            // but you can extend this yourself (see below).
            // For now, this is just a placeholder for when you want to expand.
        }

        /// <summary>
        /// Prewarm a pool by creating N instances immediately.
        /// Useful to avoid runtime spikes.
        /// </summary>
        public void Prewarm(TType type, int count)
        {
            if (!m_pools.TryGetValue(type, out ObjectPool<T> pool))
                return;

            List<T> temp = new List<T>(count);
            for (int i = 0; i < count; i++)
                temp.Add(pool.Get());

            foreach (T obj in temp)
                pool.Release(obj);
        }

        /// <summary>
        /// Despawn all instances of a given type (force clear).
        /// </summary>
        public void Clear(TType type)
        {
            if (!m_pools.TryGetValue(type, out ObjectPool<T> pool))
                return;

            pool.Clear();
        }

        /// <summary>
        /// Despawn everything in all pools (force clear).
        /// </summary>
        public void ClearAll()
        {
            foreach (ObjectPool<T> pool in m_pools.Values)
                pool.Clear();
        }

        #endregion

        private void InitialisePools()
        {
            m_pools.Clear();
            foreach (TType type in Enum.GetValues(typeof(TType)))
            {
                ObjectPool<T> pool = new ObjectPool<T>(
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

        private T Create(TType type)
        {
            T prefab = m_database.GetPrefab(type);
            if (!prefab)
            {
                Debug.LogError($"No prefab found for {typeof(T)} type {type}");
                return null;
            }
            T instance = UnityEngine.Object.Instantiate(prefab, m_parent);
            OnCreated(instance, type);
            instance.gameObject.SetActive(false);
            return instance;
        }

        protected virtual void OnCreated(T instance, TType type) { }
        protected virtual void OnGet(T instance) => instance.gameObject.SetActive(true);
        protected virtual void OnRelease(T instance) => instance.gameObject.SetActive(false);
        protected virtual void OnDestroy(T instance) => UnityEngine.Object.Destroy(instance.gameObject);
    }
}
