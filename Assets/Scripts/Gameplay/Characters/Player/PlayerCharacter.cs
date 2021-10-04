using Sirenix.OdinInspector;
using Unbowed.SO;
using Unbowed.SO.Events;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Player {
    public class PlayerCharacter : Character {
        [SerializeField] EventSO diedEventSO;
        [SerializeField] HealthChangedEventSO healthChangedEventSO;

        protected override void Start() {
            base.Start();
            
            GlobalContext.Instance.playerCharacter = this;
            
            health.HealthChanged += healthChangedEventSO.Invoke;
            healthChangedEventSO.Invoke(new HealthChangeData(health, gameObject));
        }

        protected override void OnRevive() {
            base.OnRevive();
            characterMovement.NavAgent.Warp(Vector3.zero);
            transform.rotation = new Quaternion();
        }
        
        protected override void OnDeath() {
            base.OnDeath();
            diedEventSO.Raise();
        }
        
        [HideInEditorMode]
        [Button]
        void GetHitToDeath() {
            while(!health.isDead) Hit(1, gameObject);
        }
    }
}