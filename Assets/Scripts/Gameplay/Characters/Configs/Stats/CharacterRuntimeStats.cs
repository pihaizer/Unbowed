using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Utility.Modifiers;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs.Stats {
    [Serializable]
    [HideLabel]
    [InlineProperty]
    public sealed class CharacterRuntimeStats : BaseModifiable {
        public event Action Updated;
        
        CharacterStats _baseStats;

        [field: OdinSerialize, ReadOnly, DictionaryDrawerSettings(IsReadOnly = true)]
        public Dictionary<StatType, float> Values { get; } = Enum.GetValues(typeof(StatType))
            .Cast<StatType>()
            .ToDictionary(stat => stat, stat => 0f);

        CharacterStatsModifier _defaultModifier = new CharacterStatsModifier(0);

        public CharacterRuntimeStats(CharacterStats baseStats) {
            SetBaseStats(baseStats);
            AddModifier(_defaultModifier);
        }

        public void SetBaseStats(CharacterStats baseStats) {
            _baseStats = baseStats;
            Update();
        }

        public float this[StatType type] {
            get => Values[type];
            set => Values[type] = value;
        }

        protected override void Update() {
            foreach (var statKey in Enum.GetValues(typeof(StatType)).Cast<StatType>()) {
                Values[statKey] = _baseStats[statKey];
            }

            UpdateDefaultModifier();

            base.Update();
            Updated?.Invoke();
        }

        void UpdateDefaultModifier() {
            _defaultModifier.Clear();

            _defaultModifier.Add(StatType.MinDamage, new Modifier<float>(
                Values[StatType.Strength] / 5, Operations.Add, -1));

            _defaultModifier.Add(StatType.MaxDamage, new Modifier<float>(
                Values[StatType.Strength] / 4, Operations.Add, -1));

            _defaultModifier.Add(StatType.Health, new Modifier<float>(
                Values[StatType.Endurance] * 3, Operations.Add, -1));
        }
    }
}