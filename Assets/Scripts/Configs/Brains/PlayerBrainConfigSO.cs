using System.Collections.Generic;
using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Characters.AI.Brains;
using UnityEngine;
using Zenject;

namespace Unbowed.SO.Brains {
    [CreateAssetMenu(fileName = "PlayerBrainConfig", menuName = "SO/Brains/PlayerBrainConfig")]
    public class PlayerBrainConfigSO : BrainConfigSO {
        public LayerMask navMeshLayerMask;
        public float maxWalkDistance = 100f;

        public override Brain Create() => new PlayerBrain(this);
    }
}