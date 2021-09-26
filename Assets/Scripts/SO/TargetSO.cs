using System;
using Gameplay;
using UnityEngine;

namespace SO {
    [CreateAssetMenu(fileName = "Target", menuName = "SO/Gameplay/Target", order = 0)]
    public class TargetSO : ScriptableObject {
        public event Action<ISelectable> Changed;
        public ISelectable Value => _value;
        ISelectable _value;

        public void Set(ISelectable target) {
            if (_value != target) {
                _value = target;
                Changed?.Invoke(_value);
            }
        }
    }
}