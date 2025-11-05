using System;
using System.Collections.Generic;
using UnityEngine;

namespace RSG.Core
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> s_services = new Dictionary<Type, object>();

        public static void Register<T>(T service)
        {
            Type type = typeof(T);
            if (!s_services.TryAdd( type, service ))
            {
                Debug.LogWarning($"Service of type {type} is already registered. Overwriting.");
                s_services[type] = service;
            }
        }
        
        public static T Get<T>()
        {
            Type type = typeof(T);
            if (!s_services.TryGetValue(type, out object service))
            {
                Debug.LogError($"Service of type {type} is not registered.");
                return default(T);
            }
            return (T)service;
        }
        
        public static void Clear()
        {
            s_services.Clear();
        }
    }
}