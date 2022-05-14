using System;
using System.Collections.Generic;

using Unbowed.Gameplay.Characters.Configs.Stats;

namespace Unbowed.Gameplay.Characters.Stats {
    [Serializable]
    public class StatEffectorsBundle {
        public List<StatEffector> statEffectors = new();
    }
}