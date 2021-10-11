using System.Collections.Generic;

using Sirenix.Utilities;

namespace Unbowed.Gameplay.Characters.Configs.Stats.Configs {
    [GlobalConfig("Assets/Resources/Configs")]
    public class AllStatModifiers : GlobalConfig<AllStatModifiers> {
        public List<StatModifierConfig> statModifierConfigs = new List<StatModifierConfig>();
    }
}