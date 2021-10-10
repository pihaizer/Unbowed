using System;

namespace Unbowed.Gameplay.Characters.Configs.Stats {
    [Serializable]
    public abstract class StatModifier {
        public StatType type;

        public abstract void Apply(Stat stat);

        public abstract string GetDescription();
    }

    [Serializable]
    public class AddStatModifier : StatModifier {
        public float value;

        public AddStatModifier(StatType type, float value) {
            this.type = type;
            this.value = value;
        }

        public override void Apply(Stat stat) {
            stat.modifiedValue += value;
        }

        public override string GetDescription() => $"Adds {value} to {type.name}";
    }
}