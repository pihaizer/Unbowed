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
        public Dictionary<StatType, float> Values { get; } =
            Enum.GetValues(typeof(StatType))
                .Cast<StatType>()
                .ToDictionary(stat => stat, stat => 0f);

        DefaultStatsModifier _defaultModifier = new DefaultStatsModifier();

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

            base.Update();
            Updated?.Invoke();
        }
    }
}