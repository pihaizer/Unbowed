using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unbowed.Gameplay.Items {
    [Serializable, InlineProperty, HideLabel, BoxGroup]
    public class UsableItemConfig {
        public Color color;
    }
}