using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs {
    [Serializable]
    [HideLabel]
    [InlineProperty(LabelWidth = 150)]
    [Title("Animation config")]
    public class CharacterAnimationConfig {
        [Range(0, 1)]
        public float hitMomentPercent;
    }
}