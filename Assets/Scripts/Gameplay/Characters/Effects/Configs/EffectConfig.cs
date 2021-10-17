using Sirenix.OdinInspector;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Effects.Configs {
    public abstract class EffectConfig : ScriptableObject {
        public float duration;

        public int Id => GetInstanceID();

        public abstract Effect Build();
    }
}