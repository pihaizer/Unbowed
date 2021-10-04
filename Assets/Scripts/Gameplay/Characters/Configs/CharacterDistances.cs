using Sirenix.OdinInspector;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs {
    [CreateAssetMenu]
    [InlineEditor]
    [HideLabel]
    public class CharacterDistances : SerializedScriptableObject {
        public float attackRadius = 1.5f;
        public float interactRange = 1.5f;
        public float maxChaseRange = 7f;
        public float noMoveRange = 0.15f;
    }
}