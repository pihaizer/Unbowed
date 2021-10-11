using System;
using System.Linq;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs.Stats {
    [Serializable]
    public class StatModifier {
        public StatType statType;

        public float value;

        public StatModifierType type;

        public void Apply(Stat stat) {
            switch (type) {
                case StatModifierType.Set:
                    stat.modifiedValue = value;
                    break;
                case StatModifierType.Add:
                    stat.modifiedValue += value;
                    break;
                case StatModifierType.Multiply: {
                    float modifiersSum = stat._modifiers
                        .Sum(m => m.type == StatModifierType.Multiply ? m.value : 0);
                    stat.modifiedValue *= 1 + modifiersSum;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string GetDescription() {
            return type switch {
                StatModifierType.Set => $"{statType.name} {value}",
                StatModifierType.Add => $"Adds {value} to {statType.name}",
                StatModifierType.Multiply => $"Adds {100 * value}% to {value}",
                _ => "Errored property!"
            };
        }
    }

    public enum StatModifierType {
        Set,
        Add,
        Multiply
    }
}