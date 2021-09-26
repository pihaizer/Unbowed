using UnityEngine;

namespace Gameplay {
    public readonly struct HealthChangeData {
        public readonly float oldHealth;
        public readonly float newHealth;
        public readonly GameObject source;

        public HealthChangeData(float oldHealth, float newHealth, GameObject source) {
            this.oldHealth = oldHealth;
            this.newHealth = newHealth;
            this.source = source;
        }
    }
}