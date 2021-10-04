using Unbowed.SO;
using Unbowed.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Unbowed.Gameplay.Characters {
    public class GameViewMouseSelector : MonoBehaviour {
        [SerializeField] LayerMask selectionMask;
        [SerializeField] Camera selectionCamera;
        [SerializeField] EventSystem eventSystem;

        readonly RaycastHit[] _hits = new RaycastHit[5];

        void Update() {
            UpdateMouseTargetSO();
        }

        void UpdateMouseTargetSO() {
            var mouseState = MouseState.Instance;
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

        bool TrySetTarget(ISelectable selectable) {
            if (selectable == null || !selectable.CanBeSelected()) return false;
            MouseState.Instance.SetGameViewTarget(selectable);
            return true;
        }
    }
}