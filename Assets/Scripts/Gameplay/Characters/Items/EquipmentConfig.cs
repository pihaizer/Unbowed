using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unbowed.UI;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items {
    [Serializable, InlineProperty, HideLabel]
    public class EquipmentConfig {
        public EquipmentSlot slot;

        public Dictionary<EquipmentRarity, int> rarityValues = new Dictionary<EquipmentRarity, int>();
    }
}