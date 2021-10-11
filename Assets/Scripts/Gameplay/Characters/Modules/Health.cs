using System;

using Sirenix.OdinInspector;

using Unbowed.Utility;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Modules {
    public class Health : MonoBehaviour {
        public event Action<HealthChangeData> HealthChanged;
        public event Action<DeathData> Died;
        public event Action Revived;

        [ShowInInspector]
        public int Current { get; private set; }

        public int Max { get; private set; }

        [NonSerialized, ShowInInspector] public bool isDead;

        public void Init(int maxHealth) {
            SetMax(maxHealth);
            SetCurrent(Max, null);
            isDead = false;
        }

        public void Hit(int damage, Character source) => SetCurrent(Current - damage, source);

        void Die(Character killer) {
            isDead = true;
            Died?.Invoke(new DeathData {killer = killer});
        }

        public void Revive() {
            SetCurrent(Max, null);
            isDead = false;
            Revived?.Invoke();
        }

        void SetCurrent(int newHealth, Character source) {
            if (newHealth == Current) return;

            Current = newHealth;

            HealthChanged?.Invoke(new HealthChangeData(this, source));

            if (Current <= 0) {
                Current = 0;
                Die(source);
            }
        }

        void SetMax(int newMaxHealth) {
            if (newMaxHealth == Max) return;
            Max = newMaxHealth;
            if (Current > Max) Current = Max;
            HealthChanged?.Invoke(new HealthChangeData(this, null));
        }
    }
}