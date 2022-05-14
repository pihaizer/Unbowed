using Unbowed.Gameplay.Characters.Effects.Configs;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Effects {
    public class HealOverTimeEffect : Effect {
        private readonly HealOverTimeConfig _config;
        private float _timeSinceLastHeal;
        
        public HealOverTimeEffect(int id, HealOverTimeConfig config) : base(id) => _config = config;

        protected override bool NeedToBeApplied() {
            if (_config.duration != 0) return true;
            target.health.Heal(_config.totalHeal);
            return false;
        }

        public override void Start() {
            base.Start();
            _timeSinceLastHeal = 0;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            
            if (target.health.Current == target.health.Max) {
                Stop();
                return;
            }
            
            _timeSinceLastHeal += Time.fixedDeltaTime;
            
            if (_timeSinceLastHeal < _config.duration / _config.totalHeal) return;
            
            while (_timeSinceLastHeal >= _config.duration / _config.totalHeal) {
                target.health.Heal(1);
                _timeSinceLastHeal -= _config.duration / _config.totalHeal;
            }
        }
    }
}