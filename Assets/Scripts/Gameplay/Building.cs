using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Utility;

public class Building : MonoBehaviour {
    [SerializeField] Trigger[] triggers;
    [SerializeField] MeshRenderer[] roofs;
    [SerializeField] MeshRenderer[] walls;
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material transparentMaterial;

    int _triggersCount;

    void Start() {
        foreach (var trigger in triggers) {
            trigger.Enter += TriggerOnEnter;
            trigger.Exit += TriggerOnExit;
        }
    }

    void TriggerOnEnter(Collider obj) {
        if (obj.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        _triggersCount++;
        if (_triggersCount == 1) OnPlayerInsideChanged(true);
    }

    void TriggerOnExit(Collider obj) {
        if (obj.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        _triggersCount = Mathf.Clamp(_triggersCount - 1, 0, Int32.MaxValue);
        if (_triggersCount == 0) OnPlayerInsideChanged(false);
    }

    void OnPlayerInsideChanged(bool value) {
        Debug.Log($"Player in {gameObject}: {value}");

        foreach (var roof in roofs) {
            if (value) {
                roof.material = new Material(transparentMaterial) {
                    mainTexture = defaultMaterial.mainTexture, color = defaultMaterial.color
                };
                roof.material.SetShaderPassEnabled("ShadowCaster", true);
            }
            var tweener = roof.material.DOFade(value ? 0 : 1, 0.3f);
            if (!value)
                tweener.onComplete += () => {
                    roof.material = new Material(defaultMaterial);
                };
        }

        foreach (var wall in walls) {
            if (value) {
                wall.material = new Material(transparentMaterial) {
                    mainTexture = defaultMaterial.mainTexture, color = defaultMaterial.color
                };
                wall.material.SetShaderPassEnabled("ShadowCaster", true);
            }
            var tweener = wall.material.DOFade(value ? 0.25f : 1, 0.3f);
            if (!value)
                tweener.onComplete += () => {
                    wall.material = new Material(defaultMaterial);
                };
        }
    }
}