using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unbowed.Gameplay.Characters.Configs {
    [CreateAssetMenu]
    public class CharacterConfig : SerializedScriptableObject {
        [HideLabel, InlineProperty]
        public Stats.Stats stats = new Stats.Stats();

        public CharacterDistances distances;

        public CharacterAnimationConfig animationConfig;

        public DropsConfig dropsConfig;
    }
}