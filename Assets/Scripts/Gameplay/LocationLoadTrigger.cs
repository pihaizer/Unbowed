using Unbowed.SO.Events;
using Unbowed.Utility;
using UnityEngine;

namespace Unbowed.Gameplay {
    public class LocationLoadTrigger : MonoBehaviour {
        [SerializeField] Trigger trigger;
        [SerializeField] SceneLoadRequestEventSO loadRequestEvent;
        [SerializeField] SceneLoadRequestEventSO.SceneLoadRequestData data;

        void Start() {
            trigger.Enter += RequestLocationLoad;
        }

        void RequestLocationLoad(Collider other) {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        
            loadRequestEvent.Invoke(data);
        }
    }
}
