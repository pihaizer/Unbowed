using System;
using System.Collections.Generic;
using System.Linq;
using Unbowed.Utility.Modifiers;

namespace Unbowed.Gameplay.Characters.Configs.Stats {
    [Serializable]
    public class CharacterStatsModifier : BaseModifier {
        List<KeyValuePair<StatType, Modifier<float>>> _modifiers = new List<KeyValuePair<StatType, Modifier<float>>>();

        public CharacterStatsModifier(int priority) {
            Priority = priority;
        }

        public override int Priority { get; }

        public void Clear() => _modifiers.Clear();

        public void Add(StatType type, Modifier<float> modifier) {
            _modifiers.Add(new KeyValuePair<StatType, Modifier<float>>(type, modifier));
        }

        public void Remove(StatType type) {
            _modifiers.RemoveAll(kv => kv.Key == type);
        }

        public List<Modifier<float>> this[StatType type] => 
            _modifiers.FindAll(kv => kv.Key == type)
                .Select(kv => kv.Value).ToList();

        public override void Apply(BaseModifiable baseModifiable) {
            if (!(baseModifiable is CharacterRuntimeStats stats)) return;
            foreach (var statModifier in _modifiers) {
                stats[statModifier.Key] = statModifier.Value.Operate(stats[statModifier.Key], statModifier.Value.Value);
            }
        }
    }
}