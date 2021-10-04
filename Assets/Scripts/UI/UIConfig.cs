using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.UI.Inventory;
using UnityEngine;

namespace Unbowed.UI {
    [GlobalConfig("Assets/Configs/UI")]
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

        [SerializeField] List<RarityColor> equipmentColors;

        [Title("Prefabs")]
        [AssetsOnly]
        public ItemUI itemUI;

        public Color GetEquipmentColor(EquipmentRarity rarity) =>
            equipmentColors.FirstOrDefault(rc => rc.rarity == rarity).color;

        [Serializable, InlineProperty]
        struct RarityColor {
            [HorizontalGroup, HideLabel]
            public EquipmentRarity rarity;

            [HorizontalGroup, HideLabel]
            public Color color;
        }
    }
}