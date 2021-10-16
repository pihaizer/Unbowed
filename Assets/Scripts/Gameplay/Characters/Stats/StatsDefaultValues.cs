using System.Collections.Generic;

using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Stats.Configs;

namespace Unbowed.Gameplay.Characters.Stats {
    public static class StatsDefaultValues {
        static readonly Dictionary<StatType, float> DefaultValues = new Dictionary<StatType, float>() {
            {StatType.Health, 10 },
            {StatType.MinDamage, 1},
            {StatType.MaxDamage, 1},
            {StatType.AttackTime, 1},
            {StatType.MoveSpeed, 1},
            {StatType.HitRecoveryTime, 1},
        };

        public static float Get(StatType type) {
            if (DefaultValues.ContainsKey(type))
                return DefaultValues[type];
            return 0;
        }
    }
}