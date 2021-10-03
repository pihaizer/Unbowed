using System;
using Unbowed.Gameplay.Characters.Modules;
using UnityEngine;

namespace Unbowed.Gameplay {
    public readonly struct HealthChangeData {
        public readonly Health target;
        public readonly GameObject source;

        public HealthChangeData(Health target, GameObject source) {
            this.target = target;
            this.source = source;
        }
    }
}