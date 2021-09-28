using System;
using Gameplay;
using UnityEngine;

namespace SO {
    [CreateAssetMenu(fileName = "Target", menuName = "SO/Gameplay/Target", order = 0)]
    public class MouseStateSO : ScriptableObject {
        public event Action<ISelectable> Changed;
        
        public bool isOffGameView;
        
        public ISelectable Target => _target;
        
        ISelectable _target;

        public void SetTarget(ISelectable target) {
            if (_target != target) {
                _target = target;
                Changed?.Invoke(_target);
            }
        }
    }
}