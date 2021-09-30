using TMPro;
using Unbowed.Gameplay;
using Unbowed.Gameplay.Characters;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.UI {
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
            if (_target is IHittable oldHittable && oldHittable is Character oldCharacter) {
                oldCharacter.Health.HealthChanged -= healthBar.OnHealthChanged;
            }

            _target = newTarget;
            gameObject.SetActive(_target != null);
            if (_target == null) return;

            nameText.text = _target.GetName();

            if (_target is IHittable hittable && hittable is Character character) {
                character.Health.HealthChanged += healthBar.OnHealthChanged;
                healthBar.SetHealthPercent((float) character.Health.Current / character.Health.Max);
            }
        }
    }
}