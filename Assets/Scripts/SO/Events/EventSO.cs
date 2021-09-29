using System;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Events {
    [CreateAssetMenu(fileName = "Void event", menuName = "SO/Events/Void event")]
    public class EventSO : ScriptableObject {
        event Action Actions;

        public void Raise() {
            Debug.Log($"Raising");
            Actions?.Invoke();
        }

        public void AddListener(Action listener) {
            Actions += listener;
        }

        public void RemoveListener(Action listener) {
            Actions -= listener;
        }
    }
}