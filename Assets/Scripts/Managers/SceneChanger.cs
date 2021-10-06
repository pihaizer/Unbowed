using Unbowed.SO.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unbowed.Managers {
    public class SceneChanger : MonoBehaviour {
        [SerializeField] SceneLoadRequestEventSO loadRequestEvent;
        [SerializeField] BoolEventSO loadingScreenRequestEvent;

        void Awake() {
            loadRequestEvent.AddListener(OnSceneLoadRequest);
        }

        void OnSceneLoadRequest(SceneLoadRequestEventSO.SceneLoadRequestData data) {
            if (data.hasLoadScreen) loadingScreenRequestEvent.Invoke(true);

            if (data.isLoad) {
                SceneManager.LoadSceneAsync(data.name, LoadSceneMode.Additive)
                    .completed += operation => OnSceneLoaded(data);
            }
        }

        void OnSceneLoaded(SceneLoadRequestEventSO.SceneLoadRequestData data) {
            if (data.hasLoadScreen) loadingScreenRequestEvent.Invoke(false);
        }
    }
}