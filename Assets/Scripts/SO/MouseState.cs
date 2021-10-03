using System;
using Sirenix.Utilities;
using Unbowed.Gameplay;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;

namespace Unbowed.SO {
    [CreateAssetMenu(fileName = "Target", menuName = "SO/Gameplay/Target", order = 0)]
    [GlobalConfig("Configs")]
    public class MouseState : GlobalConfig<MouseState> {
        public event Action<ISelectable> Changed;
        
        [NonSerialized] public bool isOffGameView;
        [NonSerialized] public bool blockedByDraggedItem;

        public bool BlockedByUI => isOffGameView || blockedByDraggedItem;
        
        public ISelectable Target { get; private set; }

        public void SetTarget(ISelectable target) {
            if (Target != target) {
                Target = target;
                Changed?.Invoke(Target);
            }
        }
    }
}