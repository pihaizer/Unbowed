using Unbowed.SO;
using Unbowed.Utility;
using UnityEngine;

namespace Unbowed.Gameplay {
    public class LocationLoadTrigger : MonoBehaviour {
        [SerializeField] private Trigger trigger;
        [SerializeField] private SceneConfig loadedSceneConfig;

        private void Start() {
            trigger.Enter += RequestLocationLoad;
        }

        private void RequestLocationLoad(Collider other) {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        
            ScenesConfig.Instance.Load(new SceneChangeRequest(loadedSceneConfig));
        }
    }
}
