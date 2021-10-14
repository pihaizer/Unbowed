using System;
using System.Linq;

using Sirenix.OdinInspector;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs.Stats {
    [Serializable]
    public class StatModifier : ISerializationCallbackReceiver {
        [ShowInInspector]
        public StatType StatType {
            get {
                if (!_statType) _statType = AllStatTypes.FindByName(statTypeName);
                return _statType;
            }
            set => _statType = value;
        }
        [SerializeField, HideInInspector] string statTypeName;

        public float value;

        public StatModifierType type;

        StatType _statType;

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
                StatModifierType.Set => $"{StatType.name} {value}",
                StatModifierType.Add => $"Adds {value} to {StatType.name}",
                StatModifierType.Multiply => $"Adds {100 * value}% to {value}",
                _ => "Errored property!"
            };
        }

        public void OnBeforeSerialize() {
            statTypeName = _statType.name;
        }

        public void OnAfterDeserialize() { }
    }

    public enum StatModifierType {
        Set,
        Add,
        Multiply
    }
}