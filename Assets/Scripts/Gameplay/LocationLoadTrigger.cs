using Unbowed.Managers;
using Unbowed.SO;
using Unbowed.Utility;
using UnityEngine;
using Zenject;

namespace Unbowed.Gameplay {
    public class LocationLoadTrigger : MonoBehaviour {
        [SerializeField] private Trigger trigger;
        [SerializeField] private SceneConfig loadedSceneConfig;

        [Inject] private IScenesController _scenesController;

        private void Start() {
            trigger.Enter += RequestLocationLoad;
        }

        private void RequestLocationLoad(Collider other) {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        
            _scenesController.Load(new SceneChangeRequest(loadedSceneConfig));
        }
    }
}
