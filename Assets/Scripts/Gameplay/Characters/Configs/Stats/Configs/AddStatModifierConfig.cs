using Unbowed.Utility;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs.Stats.Configs {
    [CreateAssetMenu(fileName = "Add stat modifier", menuName = "Configs/Stats/Add stat modifier")]
    public class AddStatModifierConfig : StatModifierConfig {
        public Vector2 valueRange;

        public override StatModifier Get() => new AddStatModifier(stat, VectorRandom.Range(valueRange));
    }
}