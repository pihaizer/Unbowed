using System;
using Sirenix.Utilities;
using Unbowed.Gameplay;
using UnityEngine;

namespace Unbowed.SO {
    [CreateAssetMenu(fileName = "Target", menuName = "SO/Gameplay/Target", order = 0)]
    public static class MouseContext {
        public static event Action<ISelectable> GameViewTargetChanged;
        
        public static bool isOffGameView;
        public static bool blockedByDraggedItem;

        public static bool BlockedByUI => isOffGameView || blockedByDraggedItem;
        
        public static ISelectable GameViewTarget { get; private set; }

        public static void SetGameViewTarget(ISelectable target) {
            if (GameViewTarget == target) return;
            GameViewTarget = target;
            GameViewTargetChanged?.Invoke(GameViewTarget);
        }
    }
}