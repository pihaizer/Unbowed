using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class HealthBarUI : MonoBehaviour {
    [SerializeField] Mortal target;
    [SerializeField] Image fillImage;
    [SerializeField] float animationTime;
    [SerializeField] Ease animationEase;

    void Start() {
        SetTarget(target);
    }

    public void SetTarget(Mortal newTarget) {
        if (target != null) target.currentHealth.Changed -= OnHealthChanged;
        target = newTarget;
        if (target != null) {
            target.currentHealth.Changed += OnHealthChanged;
            SetFill(target.GetHealthPercent());
        } else {
            SetFill(0);
        }
    }

    public void SetFill(float value) => fillImage.fillAmount = value;

    void OnHealthChanged(float newHealth) {
        fillImage.DOFillAmount(target.GetHealthPercent(), animationTime).SetEase(animationEase);
    }
}