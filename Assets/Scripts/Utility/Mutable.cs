using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unbowed.Utility {
    [Serializable]
    public class Mutable<T> {
        public event Action<T> Changed;
        [SerializeField, ReadOnly] private T _value;
        
        public T Value {
            get => _value;
            set => Set(value);
        }

        public void Set(T newValue) {
            if (_value == null && newValue != null || _value != null && !_value.Equals(newValue)) {
                _value = newValue;
                OnValueChanged();
            }
        }

        public void SetWithoutCallback(T newValue) => _value = newValue;

        protected void OnValueChanged() => Changed?.Invoke(Value);
        
        public static implicit operator T(Mutable<T> mutable) => mutable.Value;
    }
}
