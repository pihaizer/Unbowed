using UnityEngine;

namespace SO {
    [CreateAssetMenu(fileName = "MovementConfig", menuName = "SO/MovementConfig", order = 0)]
    public class MovementConfigSO : ScriptableObject {
        public float moveSpeed = 3f;
        public float noMoveRange = 0.15f;
        public float runSpeedMultiplier = 2f;
    }
}