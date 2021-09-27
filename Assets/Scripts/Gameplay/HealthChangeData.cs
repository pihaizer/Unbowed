using UnityEngine;

namespace Gameplay {
    public readonly struct HealthChangeData {
        public readonly Mortal target;
        public readonly float newHealth;
        public readonly GameObject source;

        public HealthChangeData(Mortal target, float newHealth, GameObject source) {
            this.target = target;
            this.newHealth = newHealth;
            this.source = source;
        }
    }
}