using Unbowed.SO;
using Unbowed.Utility;
using UnityEngine;

namespace Unbowed.Gameplay {
    public class LocationLoadTrigger : MonoBehaviour {
        [SerializeField] Trigger trigger;
        [SerializeField] SceneConfig loadedSceneConfig;

        void Start() {
            trigger.Enter += RequestLocationLoad;
        }

        void RequestLocationLoad(Collider other) {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        
            SceneDirector.Instance.Load(new SceneChangeRequest(loadedSceneConfig));
        }
    }
}
