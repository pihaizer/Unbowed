using System;
using Sirenix.OdinInspector;

using Unbowed.Gameplay.Characters.Effects.Configs;

using UnityEngine;

namespace Unbowed.Gameplay.Items {
    [Serializable, InlineProperty, HideLabel, BoxGroup]
    public class UsableItemConfig {
        public Color color;

        public EffectConfig appliedEffect;
    }
}