using System;
using Sirenix.Utilities;
using Unbowed.Gameplay;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;

namespace Unbowed.SO {
    [CreateAssetMenu(fileName = "Target", menuName = "SO/Gameplay/Target", order = 0)]
    [GlobalConfig("Assets/Configs")]
    public class MouseContext : GlobalConfig<MouseContext> {
        public event Action<ISelectable> GameViewTargetChanged;
        
        [NonSerialized] public bool isOffGameView;
        [NonSerialized] public bool blockedByDraggedItem;

        public bool BlockedByUI => isOffGameView || blockedByDraggedItem;
        
        public ISelectable GameViewTarget { get; private set; }

        public void SetGameViewTarget(ISelectable target) {
            if (GameViewTarget == target) return;
            GameViewTarget = target;
            GameViewTargetChanged?.Invoke(GameViewTarget);
        }
    }
}