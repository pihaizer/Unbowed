using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class TargetUI : MonoBehaviour {
    [SerializeField] TargetSO targetSO;
    [SerializeField] TMP_Text nameText;
    [SerializeField] HealthBarUI healthBar;

    ISelectable _target;

    void Start() {
        if (!targetSO) {
            gameObject.SetActive(false);
            return;
        }

        targetSO.Changed += OnTargetChanged;
        OnTargetChanged(targetSO.Value);
    }

    void OnTargetChanged(ISelectable newTarget) {
        _target = newTarget;
        gameObject.SetActive(_target != null);
        if (_target == null) return;

        nameText.text = _target.GetName();

        if (_target is IHittable hittable && hittable is Mortal mortal) {
            healthBar.SetTarget(mortal);
        } else {
            healthBar.SetTarget(null);
        }
    }
}