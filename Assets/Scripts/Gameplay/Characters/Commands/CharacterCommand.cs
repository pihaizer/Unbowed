using System;

namespace Unbowed.Gameplay.Characters.Commands {
    public abstract class CharacterCommand : Command {
        public virtual void Start(Character character) { }
    }
}