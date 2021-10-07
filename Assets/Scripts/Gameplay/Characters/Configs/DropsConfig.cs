using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Unbowed.Gameplay.Characters.Configs {
    [Serializable]
    [HideLabel]
    [InlineProperty(LabelWidth = 150)]
    public class DropsConfig {
        public bool hasDrops = true;

        [ShowIf(nameof(hasDrops)), MinMaxSlider(0, 100), GUIColor(nameof(GetValueColor))]
        public Vector2Int valueRange;

        [ShowIf(nameof(hasDrops)), MinMaxSlider(0, 10), GUIColor(nameof(GetAmountColor))]
        public Vector2Int amount;

        [ShowIf(nameof(hasDrops)), MinMaxSlider(0, 100), GUIColor(nameof(GetItemLevelColor))]
        public Vector2Int itemLevel;

        [ShowIf(nameof(hasDrops))]
        public List<Item> alwaysDrops;

        Color GetValueColor() => Color.Lerp(Color.white, Color.yellow,
            (valueRange.x + valueRange.y) / 200f);
        
        Color GetAmountColor() => Color.Lerp(Color.white, Color.green, 
            (amount.x + amount.y) / 200f);
        
        Color GetItemLevelColor() => Color.Lerp(Color.white, Color.blue,
            (itemLevel.x + itemLevel.y) / 200f);
    }
}