using TMPro;
using Unbowed.Gameplay;
using Unbowed.Gameplay.Characters;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.UI {
    public class TargetUI : MonoBehaviour {
        [SerializeField] TMP_Text nameText;
        [SerializeField] HealthBarUI healthBar;

        ISelectable _target;

        void Start() {
            MouseState.Instance.Changed += OnMouseStateChanged;
            OnMouseStateChanged(MouseState.Instance.Target);
        }

        void OnDestroy() {
            MouseState.Instance.Changed -= OnMouseStateChanged;
        }

        void OnMouseStateChanged(ISelectable newTarget) {
            if (_target is IHittable oldHittable && oldHittable is Character oldCharacter) {
                oldCharacter.health.HealthChanged -= healthBar.OnHealthChanged;
            }

            _target = newTarget;
            gameObject.SetActive(_target != null);
            if (_target == null) return;

            nameText.text = _target.GetName();

            if (_target is IHittable hittable && hittable is Character character) {
                character.health.HealthChanged += healthBar.OnHealthChanged;
                healthBar.SetHealthPercent((float) character.health.Current / character.health.Max);
            } else {
                healthBar.SetHealthPercent(0);
            }
        }
    }
}