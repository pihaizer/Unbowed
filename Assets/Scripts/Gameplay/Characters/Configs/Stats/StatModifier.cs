using System;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs.Stats {
    [Serializable]
    public class StatModifier {
        public StatType statType;

        public float value;

        public StatModifierType type;

        public void Apply(Stat stat) {
            if (type == StatModifierType.Add) stat.modifiedValue += value;
        }

        public string GetDescription() {
            if (type == StatModifierType.Add) {
                return $"Adds {value} to {statType.name}";
            }

            return "";
        }
    }

    public enum StatModifierType {
        Add,
        Multiply
    }
}