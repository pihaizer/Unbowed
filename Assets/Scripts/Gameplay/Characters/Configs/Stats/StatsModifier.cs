using System;
using System.Collections.Generic;

using Sirenix.Serialization;

namespace Unbowed.Gameplay.Characters.Configs.Stats {
    [Serializable]
    public class StatsModifier {
        public List<StatModifier> statModifiers = new List<StatModifier>();
    }
}