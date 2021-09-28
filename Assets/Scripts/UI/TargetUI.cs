using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using SO;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class TargetUI : MonoBehaviour {
    [SerializeField] MouseStateSO mouseStateSO;
    [SerializeField] TMP_Text nameText;
    [SerializeField] HealthBarUI healthBar;

    ISelectable _target;

    void Start() {
        if (!mouseStateSO) {
            gameObject.SetActive(false);
            return;
        }

        mouseStateSO.Changed += OnMouseStateChanged;
        OnMouseStateChanged(mouseStateSO.Target);
    }

    void OnDestroy() {
        mouseStateSO.Changed -= OnMouseStateChanged;
    }

    void OnMouseStateChanged(ISelectable newTarget) {
        if (_target is IHittable oldHittable && oldHittable is Mortal oldMortal) {
            oldMortal.HealthChanged -= healthBar.OnHealthChanged;
        }
        
        _target = newTarget;
        gameObject.SetActive(_target != null);
        if (_target == null) return;

        nameText.text = _target.GetName();

        if (_target is IHittable hittable && hittable is Mortal mortal) {
            mortal.HealthChanged += healthBar.OnHealthChanged;
            healthBar.SetHealthPercent(mortal.GetHealthPercent());
        }
    }
}