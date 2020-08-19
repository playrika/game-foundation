using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Playrika.GameFoundation.Common
{
    public class ObjectPool<T> where T : Object
    {
        private readonly Stack<T> _objects;

        private readonly T _prefab;

        private readonly Transform _parent;


        public ObjectPool(T prefab, Transform parent = null, int size = 1)
        {
            _objects = new Stack<T>();

            _prefab = prefab
                ? prefab
                : throw new Exception("Pool prefab can't be null");

            _parent = parent
                ? parent
                : new GameObject($"[Pool: {typeof(T).Name}]").transform;

            for (var i = 0; i < size; i++)
                _objects.Push(Create());
        }

        public T Pop(bool withActivate = true)
        {
            var o = _objects.Count == 0
                ? Create()
                : _objects.Pop();

            if (withActivate)
            {
                switch (o)
                {
                    case GameObject go:
                        go.SetActive(true);
                        break;

                    case MonoBehaviour monoBehaviour:
                        monoBehaviour.gameObject.SetActive(true);
                        break;
                }
            }

            return o;
        }

        public void Push(T o)
        {
            Reset(o);
            _objects.Push(o);
        }

        public void Clear()
        {
            foreach (var o in _objects)
            {
                switch (o)
                {
                    case GameObject go:
                        Object.Destroy(go);
                        break;
                    case MonoBehaviour monoBehaviour:
                        Object.Destroy(monoBehaviour.gameObject);
                        break;
                }
            }

            _objects.Clear();
        }


        private T Create()
        {
            var o = Object.Instantiate(_prefab, _parent);
            o.name = _prefab.name;

            Reset(o);
            return o;
        }

        private void Reset(object o)
        {
            GameObject gameObject;

            switch (o)
            {
                case GameObject go:
                    gameObject = go;
                    break;
                case MonoBehaviour monoBehaviour:
                    gameObject = monoBehaviour.gameObject;
                    break;
                default:
                    return;
            }

            gameObject.SetActive(false);
            gameObject.transform.SetParent(_parent, false);
        }
    }
}