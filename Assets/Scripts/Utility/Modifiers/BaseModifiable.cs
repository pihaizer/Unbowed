using System.Collections.Generic;

namespace Unbowed.Utility.Modifiers {
    public abstract class BaseModifiable {
        private readonly List<BaseModifier> _modifiers;

        protected BaseModifiable() {
            _modifiers = new List<BaseModifier>();
        }

        public virtual void AddModifier(BaseModifier baseModifier) {
            _modifiers.Add(baseModifier);
            Update();
        }

        public virtual void RemoveModifier(BaseModifier baseModifier) {
            _modifiers.Remove(baseModifier);
            Update();
        }

        public bool HasModifier(BaseModifier modifier) => _modifiers.Contains(modifier);

        protected virtual void Update() {
            _modifiers.Sort((m1, m2) => m1.Priority - m2.Priority);
            foreach (var baseModifier in _modifiers) {
                baseModifier.Apply(this);
            }
        }
    }
}