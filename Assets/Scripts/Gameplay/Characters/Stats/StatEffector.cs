using System;
using System.Linq;

using Sirenix.OdinInspector;

using Unbowed.Gameplay.Characters.Stats;
using Unbowed.Gameplay.Characters.Stats.Configs;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs.Stats {
    [Serializable]
    public class StatEffector {
        [ShowInInspector, Required]
        public StatType statType;

        public float value;

        public bool isPrimary;

        public StatModifierType type;

        public StatEffector(StatEffector other) {
            statType = other.statType;
            value = other.value;
            isPrimary = other.isPrimary;
            type = other.type;
        }

        public StatEffector() { }

        public void Apply(Stat stat) {
            switch (type) {
                case StatModifierType.Set:
                    stat.value = value;
                    break;
                case StatModifierType.Add:
                    stat.value += value;
                    break;
                case StatModifierType.Multiply: {
                    float modifiersSum = stat.effectors
                        .Sum(m => m.type == StatModifierType.Multiply ? m.value : 0);
                    stat.value *= 1 + modifiersSum;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string GetDescription() {
            return type switch {
                StatModifierType.Set => $"{statType} {value}",
                StatModifierType.Add when isPrimary => $"{statType} {value}",
                StatModifierType.Add when !isPrimary => $"Adds {value} to {statType}",
                StatModifierType.Multiply when isPrimary => $"{statType} {value * 100}%",
                StatModifierType.Multiply when !isPrimary => $"Adds {100 * value}% to {value}",
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