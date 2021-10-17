using Unbowed.Gameplay.Characters.Modules;

namespace Unbowed.Gameplay.Characters.Effects {
    public interface IEffect {
        public void Apply(Character character);
        public void Start();
        public void Update();
        public void FixedUpdate();
        public void Stop();
    }
}