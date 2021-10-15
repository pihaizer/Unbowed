using System;

using Unbowed.Gameplay.Items;

namespace Unbowed.Gameplay.Characters.Items.Configs {
    public partial class EquipmentConfig {
        public bool IsWeapon() {
            return
                type == EquipmentType.OneHandedSword ||
                type == EquipmentType.OneHandedAxe ||
                type == EquipmentType.OneHandedMace ||
                type == EquipmentType.TwoHandedSword ||
                type == EquipmentType.TwoHandedAxe ||
                type == EquipmentType.TwoHandedMace ||
                type == EquipmentType.Bow ||
                type == EquipmentType.Crossbow;
        }

        public bool IsArmor() {
            return
                type == EquipmentType.Chest ||
                type == EquipmentType.Hands ||
                type == EquipmentType.Feet ||
                type == EquipmentType.Head ||
                type == EquipmentType.Belt ||
                type == EquipmentType.Shield;
        }

        public bool IsOneHanded => IsWeapon() &&
                                   (type == EquipmentType.OneHandedSword ||
                                    type == EquipmentType.OneHandedAxe ||
                                    type == EquipmentType.OneHandedMace);

        public bool IsTwoHanded => IsWeapon() && !IsOneHanded;

        public bool IsRanged => IsWeapon() && (type == EquipmentType.Bow || type == EquipmentType.Crossbow);

        public bool IsMelee => !IsRanged;

        public bool Fits(EquipmentSlot slot) {
            return type switch {
                EquipmentType.Chest => slot == EquipmentSlot.Chest,
                EquipmentType.Hands => slot == EquipmentSlot.Hands,
                EquipmentType.Feet => slot == EquipmentSlot.Feet,
                EquipmentType.Head => slot == EquipmentSlot.Head,
                EquipmentType.Belt => slot == EquipmentSlot.Belt,
                EquipmentType.Shield => slot == EquipmentSlot.LeftHand || slot == EquipmentSlot.RightHand,
                _ when IsOneHanded => slot == EquipmentSlot.LeftHand || slot == EquipmentSlot.RightHand,
                _ when IsTwoHanded => slot == EquipmentSlot.RightHand,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}