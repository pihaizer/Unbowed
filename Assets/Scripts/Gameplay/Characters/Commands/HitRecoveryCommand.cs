using System.Collections;
using UnityEngine;
using Utility.Modifiers;

namespace Gameplay.Commands {
    public class HitRecoveryCommand : CharacterCommand {
        Character _character;
        float _remainingTime;
        
        readonly Modifier<bool> _actionsBlock = new Modifier<bool>(true);
        
        public override void Start(Character character) {
            _character = character;
            _remainingTime = _character.hitRecoveryConfig.hitRecoveryTime;
            _character.areActionsBlocked += _actionsBlock;
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
            _remainingTime -= deltaTime;
            if (_remainingTime <= 0) {
                Stop(true);
            }
        }

        public override void Stop(bool result) {
            _character.areActionsBlocked -= _actionsBlock;
            base.Stop(result);
        }

        public override string ToString() => "Recovering from hit";
    }
}