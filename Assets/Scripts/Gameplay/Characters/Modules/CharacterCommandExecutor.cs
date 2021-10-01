using Unbowed.Gameplay.Characters.Commands;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Modules {
    public class CharacterCommandExecutor : CommandExecutor {
        public Character Parent { get; private set; }

        public void Init(Character parent) {
            Parent = parent;
        }

        public override void Execute(Command command, bool isMain = true) {
            if (isMain && Parent.areActionsBlocked) return;
            base.Execute(command, isMain);
        }

        public override void ForceExecute(Command command, bool isMain = true) {
            base.ForceExecute(command, isMain);
            if (command is CharacterCommand characterCommand) {
                characterCommand.Start(Parent);
            }
        }
    }
}