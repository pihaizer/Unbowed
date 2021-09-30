using Unbowed.SO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unbowed.Gameplay.Characters {
    public class MouseSelector : MonoBehaviour {
        [SerializeField] MouseStateSO outputMouseMouseStateSO;
        [SerializeField] LayerMask selectionMask;
        [SerializeField] Camera selectionCamera;
        [SerializeField] EventSystem eventSystem;

        readonly RaycastHit[] _hits = new RaycastHit[5];

        void Update() {
            UpdateMouseTargetSO();
        }

        void UpdateMouseTargetSO() {
            var cameraRect = selectionCamera.rect;
            var screenRect = new Rect(cameraRect.x * Screen.width, cameraRect.y * Screen.height, 
                selectionCamera.pixelWidth, selectionCamera.pixelHeight);

            outputMouseMouseStateSO.isOffGameView = false;
        
            if (!screenRect.Contains(Input.mousePosition)) {
                outputMouseMouseStateSO.SetTarget(null);
                outputMouseMouseStateSO.isOffGameView = true;
                return;
            }

            if (eventSystem.IsPointerOverGameObject()) {
                outputMouseMouseStateSO.SetTarget(null);
                outputMouseMouseStateSO.isOffGameView = true;
                return;
            }

            var ray = selectionCamera.ScreenPointToRay(Input.mousePosition);
            int hitsCount = Physics.RaycastNonAlloc(ray, _hits, 100f, selectionMask);

            bool hasTarget = false;

            for (int i = 0; i < hitsCount; i++) {
                var selectable = _hits[i].transform.GetComponentInParent<ISelectable>();
                if (selectable != null && selectable.CanBeSelected()) {
                    outputMouseMouseStateSO.SetTarget(selectable);
                    hasTarget = true;
                    break;
                }
            }

            if (!hasTarget) outputMouseMouseStateSO.SetTarget(null);
        }
    }
}