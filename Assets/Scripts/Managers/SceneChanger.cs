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
            // NavMeshBuilder.CollectSources(new Bounds(Vector3.zero, Vector3.one * 500),
            //     LayerMask.GetMask("NavMeshRaycastTarget"), NavMeshCollectGeometry.RenderMeshes, 1
            //     , new List<NavMeshBuildMarkup>(), new List<NavMeshBuildSource>());
            // NavMesh.RemoveAllNavMeshData();

            // FindObjectOfType<NavMeshSurface>().BuildNavMesh();
            
            // var surfaces = FindObjectsOfType<NavMeshSurface>();
            // foreach (var surface in surfaces) {
            //     if(surface != surfaces[0]) surface.enabled = false;
            //     if(surface.navMeshData) NavMesh.AddNavMeshData(surface.navMeshData);
            // }

            // surfaces[0].AddData();
            // NavMesh.RemoveAllNavMeshData();
        }
    }
}