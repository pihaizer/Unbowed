using Sirenix.OdinInspector;
using Unbowed.SO;
using Unbowed.SO.Events;
using Unbowed.Utility.Modifiers;
using UnityEngine;

namespace Unbowed.Gameplay.Characters {
    public class PlayerCharacter : Character {
        [SerializeField] EventSO diedEventSO;
        [SerializeField] HealthChangedEventSO healthChangedEventSO;

        protected override void Start() {
            base.Start();

            GlobalContext.Instance.playerCharacter = this;
            
            Health.HealthChanged += healthChangedEventSO.Invoke;
            healthChangedEventSO.Invoke(new HealthChangeData(Health, gameObject));
        }

        protected override void OnRevive() {
            base.OnRevive();
            Movement.NavAgent.Warp(Vector3.zero);
            transform.rotation = new Quaternion();
        }
        
        protected override void OnDeath() {
            base.OnDeath();
            diedEventSO.Raise();
        }
        
        [HideInEditorMode]
        [Button]
        void GetHitToDeath() {
            while(!Health.isDead) Hit(1, gameObject);
        }
    }
}