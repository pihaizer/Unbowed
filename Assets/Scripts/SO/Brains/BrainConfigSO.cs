using Gameplay;
using Gameplay.AI;
using Gameplay.AI.Brains;
using UnityEngine;

namespace SO.Brains {
    public abstract class BrainConfigSO : ScriptableObject {
        public int ID => GetInstanceID();
        public abstract Brain Inject(Character body);
    }
}