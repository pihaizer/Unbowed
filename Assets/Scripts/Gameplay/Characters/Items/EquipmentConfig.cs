using System;
using Sirenix.OdinInspector;
using Unbowed.UI;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items {
    [Serializable, InlineProperty, HideLabel]
    public class EquipmentConfig {
        public EquipmentSlot slot;
    }
}