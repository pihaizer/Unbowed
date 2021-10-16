using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Stats;
using Unbowed.Utility.Modifiers;

namespace Unbowed.Gameplay.Characters.Commands {
    public class HitRecoveryCommand : CharacterCommand {
        Character _character;
        float _remainingTime;
        
        readonly Modifier<bool> _actionsBlock = new Modifier<bool>(true, Operations.Or);
        
        public override void Start(Character character) {
            _character = character;
            _remainingTime = _character.Stats[StatType.HitRecoveryTime];
            _character.areActionsBlocked.AddModifier(_actionsBlock);
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
            _remainingTime -= deltaTime;
            if (_remainingTime <= 0) {
                Stop(true);
            }
        }

        public override void Stop(bool result) {
            _character.areActionsBlocked.RemoveModifier(_actionsBlock);
            base.Stop(result);
        }

        public override string ToString() => "Recovering from hit";
    }
}