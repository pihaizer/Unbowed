using System.Collections.Generic;
using Unbowed.Utility.Modifiers;

namespace Unbowed.Gameplay.Characters.Configs.Stats {
    public class DefaultStatsModifier : BaseModifier {
        public override int Priority { get; } = -50;

        public override void Apply(BaseModifiable baseModifiable) {
            if (!(baseModifiable is CharacterRuntimeStats stats)) return;

            var modifiers = new Dictionary<StatType, Modifier<float>>() {
                {StatType.MinDamage, new Modifier<float>(stats[StatType.Strength] / 5, Operations.Add)},
                {StatType.MaxDamage, new Modifier<float>(stats[StatType.Strength] / 4, Operations.Add)},
                {StatType.Health, new Modifier<float>(stats[StatType.Endurance] * 3, Operations.Add)}
            };

            foreach (var statModifier in modifiers) {
                stats[statModifier.Key] = statModifier.Value.Operate(stats[statModifier.Key], statModifier.Value.Value);
            }
        }

        void ApplyModification(CharacterRuntimeStats stats, StatType type, Modifier<float> modifier) {
            stats[type] = modifier.Operate(stats[type], modifier.Value);
        }
    }
}