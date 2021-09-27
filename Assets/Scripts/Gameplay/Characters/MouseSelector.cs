using System.Collections;
using System.Collections.Generic;
using Gameplay;
using SO;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

public class MouseSelector : MonoBehaviour {
    [SerializeField] TargetSO outputMouseTargetSO;
    [SerializeField] LayerMask selectionMask;
    [SerializeField] Camera selectionCamera;

    readonly RaycastHit[] _hits = new RaycastHit[5];

    void Update() {
        UpdateMouseTargetSO();
    }

    void UpdateMouseTargetSO() {
        var ray = selectionCamera.ScreenPointToRay(Input.mousePosition);
        int hitsCount = Physics.RaycastNonAlloc(ray, _hits, 100f, selectionMask);

        bool hasTarget = false;

        for (int i = 0; i < hitsCount; i++) {
            var selectable = _hits[i].transform.GetComponentInParent<ISelectable>();
            if (selectable != null && selectable.CanBeSelected()) {
                outputMouseTargetSO.Set(selectable);
                hasTarget = true;
                break;
            }
        }

        if (!hasTarget) outputMouseTargetSO.Set(null);
    }
}