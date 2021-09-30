using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Unbowed.Utility.Modifiers {
    [ShowInInspector, InlineProperty(LabelWidth = 80)]
    public class ModifiableParameter<T> : BaseModifiable {
        [ShowInInspector]
        [HorizontalGroup]
        public T BaseValue {
            get => _baseValue;
            set => SetBaseValue(value);
        }

        [ShowInInspector]
        [HorizontalGroup]
        public T ModifiedValue => _modifiedValue;

        public event Action<T> Changed;

        T _baseValue;
        T _modifiedValue;

        public ModifiableParameter(T baseValue = default) {
            _baseValue = baseValue;
            _modifiedValue = _baseValue;
        }

        public static implicit operator T(ModifiableParameter<T> mp) => mp._modifiedValue;

        public override void AddModifier(BaseModifier baseModifier) {
            base.AddModifier(baseModifier);
            if (baseModifier is Modifier<T> modifier) {
                modifier.Changed += OnModifierChange;
            }
        }

        public override void RemoveModifier(BaseModifier baseModifier) {
            base.RemoveModifier(baseModifier);
            if (baseModifier is Modifier<T> modifier) {
                modifier.Changed -= OnModifierChange;
            }
        }

        public void ApplyModifier(Modifier<T> modifier) {
            _modifiedValue = modifier.Operate(_modifiedValue, modifier.Value);
        }

        void SetBaseValue(T value) {
            _baseValue = value;
            Update();
        }

        void OnModifierChange(T newValue) => Update();

        protected override void Update() {
            var oldValue = _modifiedValue;
            _modifiedValue = _baseValue;
            
            base.Update();

            if (!_modifiedValue.Equals(oldValue)) {
                Changed?.Invoke(_modifiedValue);
            }
        }
    }
}