using System;
using System.Collections.Generic;
using UnityEngine;

namespace RSG
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Register<T>(T service)
        {
            Type type = typeof(T);
            if (!_services.TryAdd( type, service ))
            {
                Debug.LogWarning($"Service {type.Name} is already registered.");
            }
        }

        public static void Unregister<T>(T service)
        {
            Type type = typeof(T);
            _services.Remove( type );
        }

        public static T Get<T>()
        {
            Type type = typeof(T);
            if (_services.TryGetValue(type, out object service))
            {
                return (T)service;
            }

            Debug.LogError($"Service {type.Name} not found!");
            return default;
        }
    }
}