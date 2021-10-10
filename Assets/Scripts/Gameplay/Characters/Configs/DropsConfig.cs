using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unbowed.Gameplay.Items;
using Unbowed.SO;
using Unbowed.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unbowed.Gameplay.Characters.Configs {
    [Serializable]
    [HideLabel]
    [InlineProperty(LabelWidth = 150)]
    public class DropsConfig {
        public bool hasDrops = true;

        [ShowIf(nameof(hasDrops)), MinMaxSlider(0, 10), GUIColor(nameof(GetAmountColor))]
        [OnValueChanged("@amountWeights.SetValues(VectorUtility.PointsWithin(amountRange))")]
        public Vector2Int amountRange;

        [ShowIf(nameof(hasDrops))]
        public Weights<int> amountWeights;

        [ShowIf(nameof(hasDrops)), MinMaxSlider(0, 100), GUIColor(nameof(GetItemLevelColor))]
        public Vector2Int equipmentLevelRange;

        [ShowIf(nameof(hasDrops))]
        public List<SpecialDrop> specialDrops;

        Color GetAmountColor() => Color.Lerp(Color.white, Color.green,
            (amountRange.x + amountRange.y) / 20f);

        Color GetItemLevelColor() => Color.Lerp(Color.white, Color.blue,
            (equipmentLevelRange.x + equipmentLevelRange.y) / 200f);

        public List<Item> GenerateItems() {
            var items = new List<Item>();
            if (!hasDrops) return items;

            int amount = amountWeights.Random();

            var itemLevelValidItems = ItemsConfig.Instance.allItems
                .Where(i => i.itemLevel >= equipmentLevelRange.x &&
                            i.itemLevel <= equipmentLevelRange.y).ToArray();
            if (itemLevelValidItems.Length == 0) return items;

            for (int i = 0; i < amount; i++) {
                int randomIndex = Random.Range(0, itemLevelValidItems.Length);
                var item = itemLevelValidItems[randomIndex].Generate(Random.value);
                items.Add(item);
            }

            foreach (var specialDrop in specialDrops) {
                if (Random.value < specialDrop.chance) {
                    items.Add(new Item(specialDrop.item));
                }
            }

            return items;
        }

        [Button]
        void TestAmounts() {
            for (int i = 0; i < 100; i++) {
                Debug.Log(amountWeights.Random());
            }
        }

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