using System;
using System.Collections.Generic;
using System.Linq;

using DG.Tweening;

using Sirenix.OdinInspector;
using Sirenix.Utilities;

using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Items;
using Unbowed.SO;
using Unbowed.Utility;

using UnityEngine;

using Item = Unbowed.Gameplay.Characters.Items.Item;
using Random = UnityEngine.Random;

namespace Unbowed.Gameplay.Characters.Configs {
    [Serializable]
    [HideLabel]
    [InlineProperty(LabelWidth = 150)]
    public class DropsConfig {
        public bool hasDrops = true;

        [Range(1, 5)]
        public float magicFind;

        [ShowIf(nameof(hasDrops)), MinMaxSlider(0, 10), GUIColor(nameof(GetAmountColor))]
        [OnValueChanged("@amountWeights.SetValues(VectorUtility.PointsWithin(amountRange))")]
        public Vector2Int amountRange;

        [ShowIf(nameof(hasDrops))]
        public Weights<int> amountWeights;

        [ShowIf(nameof(hasDrops)), MinMaxSlider(0, 100), GUIColor(nameof(GetItemLevelColor))]
        public Vector2Int equipmentLevelRange;

        [ShowIf(nameof(hasDrops))]
        public List<SpecialDrop> specialDrops;

        private Color GetAmountColor() => Color.Lerp(Color.white, Color.green,
            (amountRange.x + amountRange.y) / 20f);

        private Color GetItemLevelColor() => Color.Lerp(Color.white, Color.blue,
            (equipmentLevelRange.x + equipmentLevelRange.y) / 200f);


        [Serializable]
        public class SpecialDrop {
            [HorizontalGroup(0.5f), HideLabel]
            public Item item;

            [HorizontalGroup(0.5f)]
            [Range(0, 1)]
            public float chance = 1;
        }
    }
}