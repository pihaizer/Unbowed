using Sirenix.OdinInspector;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs {
    [CreateAssetMenu]
    [InlineEditor]
    [HideLabel]
    public class CharacterDistances : SerializedScriptableObject {
        public float attackRadius = 3;
        public float maxChaseRange = 7f;
        public float noMoveRange = 0.15f;
    }
}