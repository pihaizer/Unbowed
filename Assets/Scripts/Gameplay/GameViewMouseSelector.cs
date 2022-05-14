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
            var mouseState = MouseContext.Instance;
            mouseState.isOffGameView =
                !RectUtils.One.Contains(selectionCamera.ScreenToViewportPoint(Input.mousePosition)) ||
                eventSystem.IsPointerOverGameObject();

            if (mouseState.isOffGameView) {
                mouseState.SetGameViewTarget(null);
                return;
            }

            var ray = selectionCamera.ScreenPointToRay(Input.mousePosition);
            int hitsCount = Physics.RaycastNonAlloc(ray, _hits, 100f, selectionMask);

            for (int i = 0; i < hitsCount; i++) {
                var selectable = _hits[i].transform.GetComponentInParent<ISelectable>();
                if (TrySetTarget(selectable)) return;
            }

            mouseState.SetGameViewTarget(null);
        }

        private bool TrySetTarget(ISelectable selectable) {
            if (selectable == null || !selectable.CanBeSelected()) return false;
            MouseContext.Instance.SetGameViewTarget(selectable);
            return true;
        }
    }
}