using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Characters.AI.Brains;
using UnityEngine;

namespace Unbowed.SO.Brains {
    public abstract class BrainConfigSO : ScriptableObject {
        public int ID => GetInstanceID();
        public abstract Brain Create();
    }
}