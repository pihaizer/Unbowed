using System;
using Sirenix.OdinInspector;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Player {
    public class PlayerCharacter : Character {
        protected override void Start() {
            base.Start();
            ActivePlayer.SetPlayer(this);
        }

        protected override void OnRevive() {
            base.OnRevive();
        }
        
        [HideInEditorMode]
        [Button]
        void GetHitToDeath() {
            while(!health.isDead) Hit(1, gameObject);
        }
    }
}