using System;
using System.Collections.Generic;
using System.Linq;

using Sirenix.OdinInspector;

using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Stats.Configs;

namespace Unbowed.Gameplay.Characters.Stats {
    [Serializable, InlineProperty(LabelWidth = 100)]
    public class Stat {
        [HorizontalGroup(200), HideLabel]
        public StatType type;

        [HorizontalGroup, HideLabel, OnValueChanged("Update")]
        public float baseValue;

        [NonSerialized]
        public float value;

        [NonSerialized]
        public List<StatEffector> effectors = new List<StatEffector>();

        public Stat(StatType type, float baseValue) {
            this.type = type;
            this.baseValue = baseValue;
            Update();
        }

        public Stat(Stat other) : this(other.type, other.baseValue) { }

        public void AddModifier(StatEffector effector) {
            effectors ??= new List<StatEffector>();
            if (effectors.Contains(effector)) return;
            effectors.Add(effector);
        }

        public void RemoveModifier(StatEffector effector) {
            effectors?.Remove(effector);
        }

        public void Update() {
            value = baseValue;

            if (effectors == null) return;

            foreach (var statModifier in effectors.Where(stat => stat.type == StatModifierType.Set)) {
                statModifier.Apply(this);
            }

            foreach (var statModifier in effectors.Where(stat => stat.type == StatModifierType.Add)) {
                statModifier.Apply(this);
            }

            var multiplyModifiers = effectors.Where(stat => stat.type == StatModifierType.Multiply).ToList();
            // Applying only one, because it applies all multiply modifiers
            if (multiplyModifiers.Any()) multiplyModifiers.First().Apply(this);
        }

        public static implicit operator float(Stat stat) => stat.value;
    }
}