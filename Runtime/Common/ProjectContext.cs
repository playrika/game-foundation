using System;
using System.Collections.Generic;

namespace Playrika.GameFoundation.Common
{
    public static class ProjectContext
    {
        private static readonly Dictionary<Type, object> _bindings = new Dictionary<Type, object>();

        public static void Bind<T>(T service)
        {
            var type = typeof(T);

            if (_bindings.ContainsKey(type))
            {
                _bindings[type] = service;
                return;
            }

            _bindings.Add(typeof(T), service);
        }

        public static void Unbind<T>()
        {
            var type = typeof(T);
            _bindings.Remove(type);
        }

        public static void UnbindAll()
        {
            _bindings.Clear();
        }

        public static T Get<T>() where T : class
        {
            var type = typeof(T);

            if (_bindings.ContainsKey(type))
                return (T)_bindings[type];

            return null;
        }
    }
}