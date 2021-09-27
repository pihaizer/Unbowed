using UnityEngine;
using UnityEngine.Serialization;

namespace SO {
    [CreateAssetMenu(fileName = "AttackConfig", menuName = "SO/AttackConfig", order = 0)]
    public class AttackConfigSO : ScriptableObject {
        public float attackDamage = 3f;
        public float attackRadius = 3f;
        public float maxChaseRange = 7f;
        public float attackTime = 1f;

        [Range(0, 1)]
        public float hitTimeMomentPercent;
    }
}