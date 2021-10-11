using System;

using Unbowed.Gameplay.Characters.Configs.Stats;

using UnityEngine;

namespace Unbowed.Gameplay.Items {
    [Serializable]
    public class WeaponConfig {
        public WeaponType type;
        public Vector2Int damageRange = new Vector2Int(1, 1);
        public float attackTime = 1f;
        public float damageRadius = 1.5f;


        public bool IsOneHanded => type == WeaponType.OneHandedSword ||
                                   type == WeaponType.OneHandedAxe ||
                                   type == WeaponType.OneHandedMace;

        public bool IsTwoHanded => !IsOneHanded;

        public bool IsRanged => type == WeaponType.Bow || type == WeaponType.Crossbow;

        public bool IsMelee => !IsRanged;


        public void GenerateItemModifiers(Item item) {
            var minDamageModifier = new StatModifier() {
                type = StatModifierType.Set,
                statType = AllStatTypes.FindByName("MinDamage"),
                value = damageRange.x
            };
            
            var maxDamageModifier = new StatModifier() {
                type = StatModifierType.Set,
                statType = AllStatTypes.FindByName("MaxDamage"),
                value = damageRange.y
            };
            
            var attackTimeModifier = new StatModifier() {
                type = StatModifierType.Set,
                statType = AllStatTypes.FindByName("AttackTime"),
                value = attackTime
            };
            
            item.statModifiersContainer.statModifiers.Add(minDamageModifier);
            item.statModifiersContainer.statModifiers.Add(maxDamageModifier);
            item.statModifiersContainer.statModifiers.Add(attackTimeModifier);
        }

        public bool Fits(EquipmentSlot slot) {
            return type switch {
                WeaponType.OneHandedSword => slot == EquipmentSlot.LeftHand || slot == EquipmentSlot.RightHand,
                WeaponType.OneHandedAxe => slot == EquipmentSlot.LeftHand || slot == EquipmentSlot.RightHand,
                WeaponType.OneHandedMace => slot == EquipmentSlot.LeftHand || slot == EquipmentSlot.RightHand,
                WeaponType.TwoHandedSword => slot == EquipmentSlot.RightHand,
                WeaponType.TwoHandedAxe => slot == EquipmentSlot.RightHand,
                WeaponType.TwoHandedMace => slot == EquipmentSlot.RightHand,
                WeaponType.Bow => slot == EquipmentSlot.RightHand,
                WeaponType.Crossbow => slot == EquipmentSlot.RightHand,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}