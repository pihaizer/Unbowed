using System;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using Unbowed.Gameplay.Characters.Stats;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs.Stats {
    [Serializable]
    public class Stats {
        [SerializeField] 
        List<Stat> stats;
        
        public event Action Updated;

        readonly List<StatEffectorsBundle> _modifiers = new List<StatEffectorsBundle>();

        public Stats() {
            stats = new List<Stat>();
        }
        
        public Stats(Stats other) : this() {
            foreach (var stat in other.stats) {
                stats.Add(new Stat(stat));
            }
        }

        public void Update() {
            foreach (var stat in stats) stat.Update();
            Updated?.Invoke();
        }

        public void AddModifier(StatEffectorsBundle modifier) {
            if (_modifiers.Contains(modifier)) return;
            _modifiers.Add(modifier);
            foreach (var statModifier in modifier.statModifiers) {
                this[statModifier.StatType].AddModifier(statModifier);
            }
            Update();
        }

        public void RemoveModifier(StatEffectorsBundle modifier) {
            _modifiers.Remove(modifier);
            foreach (var statModifier in modifier.statModifiers) {
                this[statModifier.StatType].RemoveModifier(statModifier);
            }
            Update();
        }

        public Stat this[string statName] {
            get {
                var stat = stats.Find(s => s.type.name.Equals(statName, StringComparison.OrdinalIgnoreCase));
                if (stat == null) {
                    var type = AllStatTypes.FindByName(statName);
                    if (!type) return null;
                    stat = new Stat(type, type.defaultValue);
                    stats.Add(stat);
                }

                return stat;
            }
        }

        public Stat this[StatType type] {
            get {
                var stat = stats.Find(s => s.type == type);
                if (stat == null) {
                    if (!type) return null;
                    stat = new Stat(type, type.defaultValue);
                    stats.Add(stat);
                }

                return stat;
            }
        }
    }
}