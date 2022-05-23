using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unbowed.Gameplay.Characters.Configs {
    [CreateAssetMenu]
    public class CharacterConfig : ScriptableObject {
        [HideLabel, InlineProperty]
        public Characters.Stats.Stats stats = new Characters.Stats.Stats();

        public CharacterDistances distances;

        public CharacterAnimationConfig animationConfig;

        public DropsConfig dropsConfig;

        public ExperienceConfig ExperienceConfig;
    }
}