using System;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Events {
    [CreateAssetMenu(fileName = "Void event", menuName = "SO/Events/Void event")]
    public class EventSO : ScriptableObject {
        Action _actions;

        public void Raise() {
            _actions?.Invoke();
        }

        public void AddListener(Action listener) {
            _actions += listener;
        }

        public void RemoveListener(Action listener) {
            _actions -= listener;
        }
    }
}