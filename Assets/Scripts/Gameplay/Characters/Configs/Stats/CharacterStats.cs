using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs.Stats {
    [Serializable]
    [HideLabel]
    [InlineProperty]
    public class CharacterStats : ISerializationCallbackReceiver {
        [DictionaryDrawerSettings(IsReadOnly = true)]
        public Dictionary<StatType, float> stats = Enum.GetValues(typeof(StatType))
            .Cast<StatType>()
            .ToDictionary(stat => stat, stat => 0f);

        public float this[StatType type] => stats[type];

        public void OnBeforeSerialize() {
            var enumValues = Enum.GetValues(typeof(StatType)).Cast<StatType>().ToList();
            var keys = stats.Keys.ToList();
            for (int i = stats.Count - 1; i >= 0; i--) {
                if (!enumValues.Contains(keys[i])) {
                    stats.Remove(keys[i]);
                }
            }
        }

        public void OnAfterDeserialize() {
            var enumValues = Enum.GetValues(typeof(StatType)).Cast<StatType>().ToList();
            var keys = stats.Keys.ToList();
            for (int i = stats.Count - 1; i >= 0; i--) {
                if (!enumValues.Contains(keys[i])) {
                    stats.Remove(keys[i]);
                }
            }
            foreach (var enumValue in enumValues) {
                if (!stats.Keys.Contains(enumValue)) {
                    stats[enumValue] = 0;
                }
            }
        }
    }
}