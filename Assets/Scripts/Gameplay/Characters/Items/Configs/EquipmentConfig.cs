using System;
using System.Linq;

using Sirenix.OdinInspector;

using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Configs.Stats.Configs;
using Unbowed.Gameplay.Characters.Stats;
using Unbowed.Gameplay.Items;
using Unbowed.Utility;

namespace Unbowed.Gameplay.Characters.Items.Configs {
    [Serializable]
    public partial class EquipmentConfig {
        public EquipmentType type;

        public StatEffector[] primaryStats;

        public Weights<EquipmentRarity> rarityWeights;


        public void GenerateItemModifiers(Item item, float value) {
            item.rarity = rarityWeights.GetValue(value);
            item.statEffectorsBundle = new StatEffectorsBundle();

            foreach (var statEffector in primaryStats) {
                item.statEffectorsBundle.statModifiers.Add(new StatEffector(statEffector));
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
                item.statEffectorsBundle.statModifiers.Add(modifierConfig.Get());
            }
        }
        
        [Button] void ResetWeights() {
            rarityWeights.SetValues(Enum.GetValues(typeof(EquipmentRarity)).Cast<EquipmentRarity>().ToArray());
        }
    }
}