using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs {
    [Serializable]
    [HideLabel]
    [InlineProperty(LabelWidth = 150)]
    public class DropsConfig {
        public bool hasDrops = true;

        [ShowIf(nameof(hasDrops)), Range(0, 100), GUIColor(nameof(GetValueColor))]
        public int value;

        [ShowIf(nameof(hasDrops)), Range(0, 100), GUIColor(nameof(GetAmountColor))]
        public int amount;
        
        [ShowIf(nameof(hasDrops)), Range(0, 100), GUIColor(nameof(GetItemLevelColor))]
        public int itemLevel;

        [ShowIf(nameof(hasDrops))]
        public List<Item> alwaysDrops;

        Color GetValueColor() => Color.Lerp(Color.white, Color.yellow, value / 100f);
        Color GetAmountColor() => Color.Lerp(Color.white, Color.green, amount / 100f);
        Color GetItemLevelColor() => Color.Lerp(Color.white, Color.blue, itemLevel / 100f);
    }
}