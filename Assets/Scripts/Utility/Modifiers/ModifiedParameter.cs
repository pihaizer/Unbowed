using System;
using System.Collections.Generic;

namespace Utility.Modifiers {
    public class ModifiedParameter<T> {
        public T BaseValue {
            get => _baseValue;
            set => SetBaseValue(value);
        }

        public T ModifiedValue {
            get => _modifiedValue;
        }

        public event Action<T> Changed;

        public delegate T Interaction(T t1, T t2);

        T _baseValue;
        T _modifiedValue;
        readonly List<Modifier<T>> _modifiers = new List<Modifier<T>>();
        readonly Interaction _interactionFunc;

        public bool HasModifier(Modifier<T> modifier) => _modifiers.Contains(modifier);

        public ModifiedParameter(T baseValue, Interaction interactionFunc) {
            _baseValue = baseValue;
            _modifiedValue = _baseValue;
            _interactionFunc = interactionFunc;
        }

        public static implicit operator T(ModifiedParameter<T> mp) => mp._modifiedValue;

        public static ModifiedParameter<T> operator +(ModifiedParameter<T> mp, Modifier<T> modifier) {
            if (mp.HasModifier(modifier)) return mp;
            mp._modifiers.Add(modifier);
            modifier.OnChange += mp.OnModifierChange;
            mp.CalculateModifiedValue();
            return mp;
        }

        public static ModifiedParameter<T> operator -(ModifiedParameter<T> mp, Modifier<T> modifier) {
            if (!mp.HasModifier(modifier)) return mp;
            mp._modifiers.Remove(modifier);
            modifier.OnChange -= mp.OnModifierChange;
            mp.CalculateModifiedValue();
            return mp;
        }

        void SetBaseValue(T value) {
            _baseValue = value;
            CalculateModifiedValue();
        }

        void OnModifierChange(T newValue) => CalculateModifiedValue();

        void CalculateModifiedValue() {
            var oldValue = _modifiedValue;
            _modifiedValue = _baseValue;
            foreach (var modifier in _modifiers) _modifiedValue = _interactionFunc(_modifiedValue, modifier.Value);
            if (!_modifiedValue.Equals(oldValue)) Changed?.Invoke(_modifiedValue);
        }
    }
}