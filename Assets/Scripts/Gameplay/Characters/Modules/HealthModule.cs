using System;
using Sirenix.OdinInspector;
using Unbowed.Utility;
using UnityEngine;

namespace Unbowed.Gameplay {
    public class HealthModule {
        public event Action<HealthChangeData> HealthChanged;

        [ShowInInspector]
        public int Current { get; private set; }

        public int Max { get; private set; }

        [ShowInInspector]
        public readonly Mutable<bool> isDead = new Mutable<bool>();

        public void Init(int maxHealth) {
            SetMax(maxHealth);
            SetCurrent(Max, null);
            isDead.Set(false);
            HealthChanged += data => Debug.Log("Health changed");
        }

        public void Hit(int damage, GameObject source) => SetCurrent(Current - damage, source);

        void Die() => isDead.Set(true);

        public void Revive() {
            SetCurrent(Max, null);
            isDead.Set(false);
        }

        void SetCurrent(int newHealth, GameObject source) {
            if (newHealth == Current) return;

            Current = newHealth;

            HealthChanged?.Invoke(new HealthChangeData(this, source));

            if (Current <= 0) {
                Current = 0;
                Die();
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