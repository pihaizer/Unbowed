namespace Unbowed.Gameplay.Characters.Effects {
    public abstract class Effect : IEffect {
        protected Character target;
        
        public int Id { get; private set; }

        protected Effect(int id) {
            Id = id;
        }

        public void Apply(Character character) {
            target = character;
            if (NeedToBeApplied()) Start();
        }

        protected virtual bool NeedToBeApplied() => true;

        public virtual void Start() => target.effects.Add(this);

        public virtual void Update() { }

        public virtual void FixedUpdate() { }

        public virtual void Stop() => target.effects.Remove(this);
    }
}