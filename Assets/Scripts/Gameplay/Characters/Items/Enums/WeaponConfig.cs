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
            var minDamageModifier = new StatEffector() {
                type = StatModifierType.Set,
                StatType = AllStatTypes.FindByName("MinDamage"),
                value = damageRange.x,
                isPrimary = true
            };
            
            var maxDamageModifier = new StatEffector() {
                type = StatModifierType.Set,
                StatType = AllStatTypes.FindByName("MaxDamage"),
                value = damageRange.y,
                isPrimary = true
            };
            
            var attackTimeModifier = new StatEffector() {
                type = StatModifierType.Set,
                StatType = AllStatTypes.FindByName("AttackTime"),
                value = attackTime,
                isPrimary = true
            };
            
            item.statEffectorsBundle.statModifiers.Add(minDamageModifier);
            item.statEffectorsBundle.statModifiers.Add(maxDamageModifier);
            item.statEffectorsBundle.statModifiers.Add(attackTimeModifier);
        }

        public bool Fits(EquipmentSlot slot) {
            if (IsOneHanded)
                return slot == EquipmentSlot.LeftHand || slot == EquipmentSlot.RightHand;
            else
                return slot == EquipmentSlot.RightHand;
        }
    }
}