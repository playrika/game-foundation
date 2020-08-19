using System;
using System.Collections.Generic;

namespace Playrika.GameFoundation.Common
{
    public class StatefulEvent<T>
    {
        public bool isInvoked { get; private set; }

        public T value { get; protected set; }

        private readonly List<Action<T>> _listeners = new List<Action<T>>();


        public StatefulEvent() { }

        public StatefulEvent(T value)
        {
            this.value = value;
        }


        public void AddListener(Action<T> listener)
        {
            _listeners.Add(listener);

            if (isInvoked)
                listener?.Invoke(value);
        }

        public void RemoveListener(Action<T> listener)
        {
            if (!_listeners.Contains(listener))
                return;

            _listeners.Remove(listener);
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
        }


        public void Invoke(T val)
        {
            value = val;

            foreach (var listener in _listeners)
                listener?.Invoke(value);

            isInvoked = true;
        }
    }
}