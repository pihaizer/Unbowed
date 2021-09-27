using System;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Events {
    [CreateAssetMenu(fileName = "Void event", menuName = "SO/Events/Void event")]
    public class EventSO : ScriptableObject {
        readonly List<EventListener> _listeners = new List<EventListener>();

        public void Raise() {
            for (int i = _listeners.Count - 1; i >= 0; i--) _listeners[i].OnEventRaised();
        }

        public void RegisterListener(EventListener listener) {
            _listeners.Add(listener);
        }

        public void UnregisterListener(EventListener listener) {
            _listeners.Remove(listener);
        }
    }
}