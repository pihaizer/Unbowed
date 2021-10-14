using System;

using Unbowed.Gameplay.Characters.Configs.Stats;

namespace Unbowed.Gameplay.Items {
    [Serializable]
    public class ArmorConfig {
        public ArmorType type;
        public int defense;

        
        public void GenerateItemModifiers(Item item) {
            var armorModifier = new StatEffector() {
                type = StatModifierType.Add,
                StatType = AllStatTypes.FindByName("Defense"),
                value = defense
            };
            
            item.statEffectorsBundle.statModifiers.Add(armorModifier);
        }
        
        public bool Fits(EquipmentSlot slot) {
            return type switch {
                ArmorType.Chest => slot == EquipmentSlot.Chest,
                ArmorType.Hands => slot == EquipmentSlot.Hands,
                ArmorType.Feet => slot == EquipmentSlot.Feet,
                ArmorType.Head => slot == EquipmentSlot.Head,
                ArmorType.Belt => slot == EquipmentSlot.Belt,
                ArmorType.Shield => slot == EquipmentSlot.LeftHand || slot == EquipmentSlot.RightHand,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}