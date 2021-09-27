using System;
using SO;
using UnityEngine;
using Utility;

namespace Gameplay {
    public abstract class Mortal : MonoBehaviour, ISelectable, IHittable {
        ///<summary>First parameter - old health, second parameter - new health</summary>
        public event Action<HealthChangeData> HealthChanged;

        [SerializeField] protected HealthConfigSO healthConfig;

        public readonly Mutable<float> currentHealth = new Mutable<float>();
        public readonly Mutable<bool> isDead = new Mutable<bool>();

        protected virtual void Start() {
            currentHealth.Set(healthConfig.maxHealth);
            isDead.Set(false);
        }

        public virtual void Hit(float damage, GameObject source) {
            SetHealth(currentHealth - damage, source);
        }

        protected virtual void SetHealth(float newHealth, GameObject source) {
            if (Math.Abs(newHealth - currentHealth) < Mathf.Epsilon) return;
        
            float oldHealth = currentHealth;
        
            currentHealth.Set(newHealth);
        
            HealthChanged?.Invoke(new HealthChangeData(this, currentHealth, source));
        
            if (currentHealth <= 0) {
                currentHealth.Set(0);
                Die();
            }
        }

        public bool CanBeHit() => !isDead;

        public virtual void Revive() {
            gameObject.SetActive(true);
            currentHealth.Set(healthConfig.maxHealth);
            isDead.Set(false);
        }

        protected virtual void Die() {
            isDead.Set(true);
        }
    
        public float GetHealthPercent() => currentHealth / healthConfig.maxHealth;

        public GameObject GetGameObject() => gameObject;

        public string GetName() => gameObject.name;

        public bool CanBeSelected() => !isDead;
    }
}