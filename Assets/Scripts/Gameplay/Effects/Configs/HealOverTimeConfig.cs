using UnityEngine;

namespace Unbowed.Gameplay.Characters.Effects.Configs {
    [CreateAssetMenu(fileName = "HealOverTimeConfig", menuName = "Configs/Effects/HealOverTimeConfig", order = 0)]
    public class HealOverTimeConfig : EffectConfig {
        public int totalHeal;
        
        public override Effect Build() => new HealOverTimeEffect(Id, this);
    }
}