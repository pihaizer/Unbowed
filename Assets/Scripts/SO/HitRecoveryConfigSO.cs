using UnityEngine;
using UnityEngine.Serialization;

namespace SO {
    [CreateAssetMenu(fileName = "HitRecoveryConfig", menuName = "SO/HitRecoveryConfig", order = 0)]
    public class HitRecoveryConfigSO : ScriptableObject {
        [Range(0, 1f)]
        public float stunDamagePercentThreshold = 0.125f;

        public float hitRecoveryTime = 0.5f;
    }
}