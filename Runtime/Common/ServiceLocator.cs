using System;
using System.Collections.Generic;

namespace Playrika.GameFoundation.Common
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Bind<T>(T service)
        {
            var type = typeof(T);

            if (_services.ContainsKey(type))
            {
                _services[type] = service;
                return;
            }

            _services.Add(typeof(T), service);
        }

        public static void Unbind<T>()
        {
            var type = typeof(T);
            _services.Remove(type);
        }

        public static void UnbindAll()
        {
            _services.Clear();
        }

        public static T Get<T>() where T : class
        {
            var type = typeof(T);

            if (_services.ContainsKey(type))
                return (T)_services[type];

            return null;
        }
    }
}