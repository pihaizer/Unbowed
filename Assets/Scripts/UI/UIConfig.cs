using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unbowed.Gameplay.Items;
using Unbowed.UI.Gameplay.Inventory;
using UnityEngine;

namespace Unbowed.UI {
    [GlobalConfig("Assets/Resources/Configs")]
    public class UIConfig : GlobalConfig<UIConfig> {
        [Title("Colors")]
        public Color defaultSlotColor;

        public Color positiveSlotColor;
        [Range(0, 1)] public float positiveBlend;
        public Color errorSlotColor;
        [Range(0, 1)] public float errorBlend;
        public Color hoverSlotColor;
        [Range(0, 1)] public float hoverBlend;
        public Color replaceSlotColor;
        [Range(0, 1)] public float replaceBlend;

        [SerializeField] private List<RarityColor> equipmentColors = Enum.GetValues(typeof(EquipmentRarity))
            .Cast<EquipmentRarity>().Select(rarity => new RarityColor {rarity = rarity}).ToList();

        [Title("Prefabs")]
        [AssetsOnly]
        public ItemUI itemUI;

        public Color GetEquipmentColor(EquipmentRarity rarity) =>
            equipmentColors.FirstOrDefault(rc => rc.rarity == rarity).color;

        [Serializable, InlineProperty]
        private struct RarityColor {
            [HorizontalGroup, HideLabel]
            public EquipmentRarity rarity;

            [HorizontalGroup, HideLabel]
            public Color color;
        }
    }
}