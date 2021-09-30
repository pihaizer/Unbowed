using System;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs {
    [Serializable]
    public class CharacterAnimationConfig {
        [Range(0, 1)]
        public float hitMomentPercent;
    }
}