using System;

namespace Gameplay.Commands {
    public abstract class CharacterCommand {
        public event Action<bool> Executed;
        
        public bool Result { get; private set; }

        public virtual void Start(Character character) { }

        public virtual void Update(float deltaTime) { }

        public virtual void Stop(bool result) {
            Result = result;
            Executed?.Invoke(result);
        }
    }
}