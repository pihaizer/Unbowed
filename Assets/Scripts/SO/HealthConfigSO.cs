using UnityEngine;

namespace SO {
    [CreateAssetMenu(fileName = "HealthConfig", menuName = "SO/HealthConfig")]
    public class HealthConfigSO : ScriptableObject {
        public float maxHealth;
    }
}