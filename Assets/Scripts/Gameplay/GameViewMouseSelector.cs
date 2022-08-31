using Unbowed.SO;
using Unbowed.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Unbowed.Gameplay.Characters {
    public class GameViewMouseSelector : MonoBehaviour {
        [SerializeField] private LayerMask selectionMask;
        [SerializeField] private Camera selectionCamera;
        [SerializeField] private EventSystem eventSystem;

        private readonly RaycastHit[] _hits = new RaycastHit[5];

        private void Update() {
            UpdateMouseTargetSO();
        }

        private void UpdateMouseTargetSO() {
            MouseContext.isOffGameView =
                !RectUtils.One.Contains(selectionCamera.ScreenToViewportPoint(Input.mousePosition)) ||
                eventSystem.IsPointerOverGameObject();

            if (MouseContext.isOffGameView) {
                MouseContext.SetGameViewTarget(null);
                return;
            }

            var ray = selectionCamera.ScreenPointToRay(Input.mousePosition);
            int hitsCount = Physics.RaycastNonAlloc(ray, _hits, 100f, selectionMask);

            for (int i = 0; i < hitsCount; i++) {
                var selectable = _hits[i].transform.GetComponentInParent<ISelectable>();
                if (TrySetTarget(selectable)) return;
            }

            MouseContext.SetGameViewTarget(null);
        }

        private bool TrySetTarget(ISelectable selectable) {
            if (selectable == null || !selectable.CanBeSelected()) return false;
            MouseContext.SetGameViewTarget(selectable);
            return true;
        }
    }
}