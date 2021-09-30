using System;
using UnityEngine;

namespace Unbowed.SO.Events {
    [CreateAssetMenu(fileName = "Void event", menuName = "SO/Events/Void event")]
    public class EventSO : ScriptableObject {
        event Action Actions;

        public void Raise() {
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