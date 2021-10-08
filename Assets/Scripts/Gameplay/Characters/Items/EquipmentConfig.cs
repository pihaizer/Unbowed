using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unbowed.UI;
using Unbowed.Utility;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items {
    [Serializable]
    public class EquipmentConfig {
        public EquipmentSlot slot;

        public Weights<EquipmentRarity> rarityWeights;

        [Button]
        void ResetWeights() {
            rarityWeights.SetValues(Enum.GetValues(typeof(EquipmentRarity)).Cast<EquipmentRarity>().ToArray());
        }
    }
}