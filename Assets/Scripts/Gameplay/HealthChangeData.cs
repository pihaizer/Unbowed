using System;

using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Characters.Modules;
using UnityEngine;

namespace Unbowed.Gameplay {
    public readonly struct HealthChangeData {
        public readonly Health target;
        public readonly Character source;

        public HealthChangeData(Health target, Character source) {
            this.target = target;
            this.source = source;
        }
    }
}