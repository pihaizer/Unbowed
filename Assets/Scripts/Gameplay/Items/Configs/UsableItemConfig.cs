using Unbowed.Gameplay.Characters.Effects.Configs;
using Unbowed.Gameplay.Items;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items.Configs {
    [CreateAssetMenu]
    public class UsableItemConfig : ItemConfig {
        public Color color;

        public EffectConfig appliedEffect;
        
        public override Item Generate(float value)
        {
            return new UsableItem(this);
        }
    }
}