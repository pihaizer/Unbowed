using System;
using System.Linq;

using Sirenix.OdinInspector;

using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Configs.Stats.Configs;
using Unbowed.Utility;

using UnityEngine;

namespace Unbowed.Gameplay.Items {
    [Serializable]
    public class EquipmentConfig {
        public EquipmentType type;

        [ShowIf(nameof(type), EquipmentType.Armor)]
        public ArmorConfig armorConfig;

        [ShowIf(nameof(type), EquipmentType.Weapon)]
        public WeaponConfig weaponConfig;

        public Weights<EquipmentRarity> rarityWeights;


        public void GenerateItemModifiers(Item item, float value) {
            if (!item.IsEquipment) return;

            item.rarity = rarityWeights.GetValue(value);
            item.statModifiersContainer = new StatModifiersContainer();

            if (type == EquipmentType.Weapon) {
                weaponConfig.GenerateItemModifiers(item);
            } else if (type == EquipmentType.Armor) {
                armorConfig.GenerateItemModifiers(item);
            }

            var modifiers = AllStatModifiers.Instance.statModifierConfigs
                .Where(mod => item.Config.itemLevel >= mod.itemLevelRange.x &&
                              item.Config.itemLevel <= mod.itemLevelRange.y).ToArray();

            int modifiersAmount = item.rarity switch {
                EquipmentRarity.Poor => 0,
                EquipmentRarity.Normal => 1,
                EquipmentRarity.Good => 2,
                EquipmentRarity.Rare => 3,
                EquipmentRarity.Unique => 4,
                _ => throw new ArgumentOutOfRangeException()
            };

            for (int i = 0; i < modifiersAmount; i++) {
                int index = UnityEngine.Random.Range(0, modifiers.Length);
                var modifierConfig = modifiers[index];
                item.statModifiersContainer.statModifiers.Add(modifierConfig.Get());
            }
        }

        public bool Fits(EquipmentSlot slot) {
            return type switch {
                EquipmentType.Armor => armorConfig.Fits(slot),
                EquipmentType.Weapon => weaponConfig.Fits(slot),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        [Button] void ResetWeights() {
            rarityWeights.SetValues(Enum.GetValues(typeof(EquipmentRarity)).Cast<EquipmentRarity>().ToArray());
        }
    }
}