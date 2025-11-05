using RSG.Core;
using System;
using System.Collections.Generic;

namespace RSG.Core
{
    public abstract class PoolRegistryBase : MonoSingleton<PoolRegistryBase>
    {
        private readonly Dictionary<Type, object> m_pools = new Dictionary<Type, object>();

        protected void RegisterPool<T>(T pool) where T : class =>
            m_pools[typeof(T)] = pool;

        protected void UnregisterPool<T>() where T : class =>
            m_pools.Remove(typeof(T));

        public T GetPool<T>() where T : class =>
            m_pools.TryGetValue(typeof(T), out object pool) ? pool as T : null;

        // Hook for child registries to implement
        protected abstract void RegisterPools();

        protected override void Init()
        {
            RegisterPools();
        }
    }
}