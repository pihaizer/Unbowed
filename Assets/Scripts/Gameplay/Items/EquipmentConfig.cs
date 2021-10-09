using System;
using System.Linq;
using Sirenix.OdinInspector;
using Unbowed.Utility;

namespace Unbowed.Gameplay.Items {
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