using System;
using Sirenix.OdinInspector;
using Unbowed.UI;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items {
    [Serializable, InlineProperty, HideLabel]
    public class EquipmentConfig {
        public EquipmentSlot slot;
        public EquipmentRarity rarity;

        public Color Color => UIConfig.Instance.GetEquipmentColor(rarity);
    }
}