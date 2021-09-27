using UnityEngine;
using UnityEngine.Events;

namespace SO.Events.Listeners {
    public class BoolEventListener : MonoBehaviour {
        [SerializeField] BoolEventSO @event;
        [SerializeField] UnityEvent<bool> response;


        void OnEnable() {
            @event.AddListener(response.Invoke);
        }

        void OnDisable() {
            @event.RemoveListener(response.Invoke);
        }
    }
}