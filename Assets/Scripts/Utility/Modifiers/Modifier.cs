using System;

namespace Utility.Modifiers {
    public class Modifier<T> {
        public T Value { get => _value; set => SetValue(value); }
        public Action<T> OnChange;
        T _value;
        public Modifier(T value) {
            _value = value;
        }
        public void SetValue(T value) {
            if (_value.Equals(value)) return;
            _value = value;
            OnChange?.Invoke(value);
        }
    }
}