using System;
using Unbowed.Gameplay;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;

namespace Unbowed.SO {
    [CreateAssetMenu(fileName = "Target", menuName = "SO/Gameplay/Target", order = 0)]
    public class MouseStateSO : ScriptableObject {
        public event Action<ISelectable> Changed;
        
        [NonSerialized] public bool isOffGameView;
        
        public ISelectable Target { get; private set; }

        public void SetTarget(ISelectable target) {
            if (Target != target) {
                Target = target;
                Changed?.Invoke(Target);
            }
        }
    }
}