using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Stats
{
    [Serializable]
    public class Stats
    {
        [SerializeField] private List<Stat> stats;

        public event Action Updated;

        private readonly List<StatEffectorsBundle> _modifiers = new List<StatEffectorsBundle>();

        public Stats()
        {
            stats = new List<Stat>();
        }

        public Stats(Stats other) : this()
        {
            foreach (var stat in other.stats)
            {
                stats.Add(new Stat(stat));
            }
        }

        public void Update()
        {
            foreach (var stat in stats) stat.Update();
            Updated?.Invoke();
        }

        public void AddModifier(StatEffectorsBundle modifier)
        {
            if (modifier == null || _modifiers.Contains(modifier)) return;
            _modifiers.Add(modifier);
            foreach (var statModifier in modifier.statEffectors)
            {
                this[statModifier.statType].AddModifier(statModifier);
            }

            Update();
        }

        public void RemoveModifier(StatEffectorsBundle modifier)
        {
            if (modifier == null) return;
            _modifiers.Remove(modifier);
            foreach (var statModifier in modifier.statEffectors)
            {
                this[statModifier.statType].RemoveModifier(statModifier);
            }

            Update();
        }

        public Stat this[StatType type]
        {
            get
            {
                var stat = stats.Find(s => s.type == type);
                if (stat == null)
                {
                    stat = new Stat(type, StatsDefaultValues.Get(type));
                    stats.Add(stat);
                }

                return stat;
            }
        }
    }
}