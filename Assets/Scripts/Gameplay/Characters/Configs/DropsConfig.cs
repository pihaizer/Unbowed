using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs {
    [Serializable]
    [HideLabel]
    [InlineProperty(LabelWidth = 150)]
    [Title("Drops config")]
    public class DropsConfig {
        public bool hasDrops = true;
        
        [ShowIf(nameof(hasDrops)), Range(0, 100)]
        public int dropValue;

        [ShowIf(nameof(hasDrops)), Range(0, 100)]
        public int dropAmount;

        [ShowIf(nameof(hasDrops))]
        public List<Item> alwaysDrops;
    }
}