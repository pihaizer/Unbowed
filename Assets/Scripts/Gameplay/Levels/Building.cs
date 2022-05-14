using System;
using DG.Tweening;
using Unbowed.Utility;
using UnityEngine;

namespace Unbowed.Gameplay.Levels {
    public class Building : MonoBehaviour {
        [SerializeField] private Trigger[] triggers;
        [SerializeField] private MeshRenderer[] roofs;
        [SerializeField] private MeshRenderer[] walls;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material transparentMaterial;

        private int _triggersCount;

        private void Start() {
            foreach (var trigger in triggers) {
                trigger.Enter += TriggerOnEnter;
                trigger.Exit += TriggerOnExit;
            }
        }

        private void TriggerOnEnter(Collider obj) {
            if (obj.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            _triggersCount++;
            if (_triggersCount == 1) OnPlayerInsideChanged(true);
        }

        private void TriggerOnExit(Collider obj) {
            if (obj.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            _triggersCount = Mathf.Clamp(_triggersCount - 1, 0, Int32.MaxValue);
            if (_triggersCount == 0) OnPlayerInsideChanged(false);
        }

        private void OnPlayerInsideChanged(bool value) {
            Debug.Log($"Player in {gameObject}: {value}");

            foreach (var roof in roofs) {
                for (int i = 0; i < roof.materials.Length; i++) {
                    if (value) {
                        roof.materials[i] = new Material(transparentMaterial) {
                            mainTexture = defaultMaterial.mainTexture, color = defaultMaterial.color,
                        };
                        roof.materials[i].SetShaderPassEnabled("ShadowCaster", true);
                    }
                    var tweener = roof.materials[i].DOFade(value ? 0 : 1, 0.3f);
                    if (!value) {
                        int i1 = i;
                        tweener.onComplete += () => {
                            roof.materials[i1] = new Material(defaultMaterial);
                        };
                    }
                }
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
}