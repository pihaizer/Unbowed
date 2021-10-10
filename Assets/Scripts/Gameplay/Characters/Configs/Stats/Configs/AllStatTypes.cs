using System;
using System.Collections.Generic;

using Sirenix.Utilities;

using UnityEditor;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs.Stats {
    [GlobalConfig("Assets/Resources/Configs")]
    public class AllStatTypes : GlobalConfig<AllStatTypes> {
        public readonly List<StatType> statTypes = new List<StatType>();

        public static StatType FindByName(string name) {
            var type = Instance.statTypes.Find(stat => stat.name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return type;
        }
    }
}