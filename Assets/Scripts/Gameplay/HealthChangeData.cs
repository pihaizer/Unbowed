using System;
using UnityEngine;

namespace Unbowed.Gameplay {
    public readonly struct HealthChangeData {
        public readonly HealthModule target;
        public readonly GameObject source;

        public HealthChangeData(HealthModule target, GameObject source) {
            this.target = target;
            this.source = source;
        }
    }
}