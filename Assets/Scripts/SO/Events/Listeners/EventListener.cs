using UnityEngine;
using UnityEngine.Events;

namespace SO.Events {
    public class EventListener : MonoBehaviour {
        public EventSO Event;
        public UnityEvent Response;

        void OnEnable() {
            Event.RegisterListener(this);
        }

        void OnDisable() {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised() {
            Response.Invoke();
        }
    }
}