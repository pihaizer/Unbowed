using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Configs.Stats.Configs;
using Unbowed.Gameplay.Characters.Stats;
using Unbowed.Gameplay.Characters.Stats.Configs;
using Unbowed.Gameplay.Items;
using Unbowed.Utility;
using Zenject;

namespace Unbowed.Gameplay.Characters.Items.Configs
{
    public abstract class EquipmentConfig : ItemConfig
    {
        public StatEffector[] primaryStats;

        public Weights<EquipmentRarity> rarityWeights;

        [Inject] private AllStatModifiers _modifiers;

        protected void GenerateStats(Equipment item, float value)
        {
            EquipmentRarity rarity = rarityWeights.GetValue(value);
            List<StatEffector> effectors = GenerateStatEffectors(rarity);

            item.Stats = new StatEffectorsBundle
            {
                statEffectors = effectors
            };
        }

        public void GenerateItemModifiers(Equipment item, float value)
        {
            item.Rarity = rarityWeights.GetValue(value);
            item.Stats = new StatEffectorsBundle();

            foreach (var statEffector in primaryStats)
            {
                item.Stats.statEffectors.Add(new StatEffector(statEffector));
            }

            var modifiers = _modifiers.statModifierConfigs
                .Where(mod => item.Config.itemLevel >= mod.itemLevelRange.x &&
                              item.Config.itemLevel <= mod.itemLevelRange.y).ToArray();

            int modifiersAmount = item.Rarity switch
            {
                EquipmentRarity.Poor => 0,
                EquipmentRarity.Normal => 1,
                EquipmentRarity.Good => 2,
                EquipmentRarity.Rare => 3,
                EquipmentRarity.Unique => 4,
                _ => throw new ArgumentOutOfRangeException()
            };

            for (int i = 0; i < modifiersAmount; i++)
            {
                int index = UnityEngine.Random.Range(0, modifiers.Length);
                var modifierConfig = modifiers[index];
                item.Stats.statEffectors.Add(modifierConfig.Get());
            }
        }

        public virtual List<StatEffector> GenerateStatEffectors(EquipmentRarity rarity)
        {
            StatModifierConfig[] modifiers = _modifiers.statModifierConfigs
                .Where(mod => itemLevel >= mod.itemLevelRange.x &&
                              itemLevel <= mod.itemLevelRange.y).ToArray();

            int modifiersAmount = rarity switch
            {
                EquipmentRarity.Poor => 0,
                EquipmentRarity.Normal => 1,
                EquipmentRarity.Good => 2,
                EquipmentRarity.Rare => 3,
                EquipmentRarity.Unique => 4,
                _ => throw new ArgumentOutOfRangeException()
            };

            var effectors = new List<StatEffector>();

            for (int i = 0; i < modifiersAmount; i++)
            {
                int index = UnityEngine.Random.Range(0, modifiers.Length);
                StatModifierConfig modifierConfig = modifiers[index];
                effectors.Add(modifierConfig.Get());
            }

            return effectors;
        }

        [Button]
        private void ResetWeights()
        {
            rarityWeights.SetValues(Enum.GetValues(typeof(EquipmentRarity)).Cast<EquipmentRarity>().ToArray());
        }

        public abstract bool Fits(EquipmentSlot slot);
    }
}