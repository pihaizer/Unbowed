using System;
using UnityEngine;

namespace Unbowed.Utility.Modifiers {
    public class Modifier<T> : BaseModifier {
        public T Value {
            get => _value;
            set => SetValue(value);
        }
        
        public event Action<T> Changed;

        public override int Priority { get; }

        public Operation Operate { get; }

        public delegate T Operation(T t1, T t2);
        
        T _value;

        public Modifier(T value, Operation operation, int priority = 0) {
            _value = value;
            Operate = operation;
            Priority = priority;
        }

        public override void Apply(BaseModifiable baseModifiable) {
            if (baseModifiable is ModifiableParameter<T> modifiableParameter) {
                modifiableParameter.ApplyModifier(this);
            }
        }

        void SetValue(T value) {
            if (_value.Equals(value)) return;
            _value = value;
            Changed?.Invoke(value);
        }
    }
}