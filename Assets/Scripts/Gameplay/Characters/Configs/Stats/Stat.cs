using System;
using System.Collections.Generic;

using Sirenix.OdinInspector;

namespace Unbowed.Gameplay.Characters.Configs.Stats {
    [Serializable, InlineProperty(LabelWidth = 100)]
    public class Stat {
        [HorizontalGroup(200), HideLabel]
        public StatType type;

        [HorizontalGroup, HideLabel, OnValueChanged("Update")]
        public float baseValue;

        [NonSerialized, ShowInInspector, HorizontalGroup]
        public float modifiedValue;

        [NonSerialized, ShowInInspector, HorizontalGroup]
        List<StatModifier> _modifiers = new List<StatModifier>();

        public Stat(StatType type, float baseValue) {
            this.type = type;
            this.baseValue = baseValue;
            Update();
        }

        public Stat(Stat other) : this(other.type, other.baseValue) { }

        public void AddModifier(StatModifier modifier) {
            _modifiers ??= new List<StatModifier>();
            if (_modifiers.Contains(modifier)) return;
            _modifiers.Add(modifier);
        }

        public void RemoveModifier(StatModifier modifier) {
            _modifiers?.Remove(modifier);
        }

        public void Update() {
            modifiedValue = baseValue;

            if (_modifiers == null) return;
            foreach (var statModifier in _modifiers) {
                statModifier.Apply(this);
            }
        }

        public static implicit operator float(Stat stat) => stat.modifiedValue;
    }
}