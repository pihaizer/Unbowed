using System;
using UnityEngine;

namespace SO.Events {
    public abstract class ParameterEventSO<T> : ScriptableObject {
        Action<T> _action;

        public void Invoke(T data) => _action?.Invoke(data);

        public void AddListener(Action<T> action) {
            _action += action;
        }

        public void RemoveListener(Action<T> action) {
            _action -= action;
        }

        public static implicit operator Action<T>(ParameterEventSO<T> parameterEventSO) => 
            parameterEventSO._action;
    }
}
