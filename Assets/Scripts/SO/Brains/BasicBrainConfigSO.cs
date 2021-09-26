using System.Collections.Generic;
using Gameplay;
using Gameplay.AI;
using Gameplay.AI.Brains;
using UnityEngine;

namespace SO.Brains {
    [CreateAssetMenu(fileName = "BasicBrainConfig", menuName = "SO/Brains/BasicBrainConfig")]
    public class BasicBrainConfigSO : BrainConfigSO {
        public float playerAggroRange;
        public Vector2 newPointsRadiusRange = new Vector2(1, 5);
        public float maxWalkDistance = 10f;
        public float maxWalkTime = 999;
        public Vector2 idleAfterMoveTimeRange = new Vector2(1, 2);
        public List<CharacterTypeSO> targetTypes;
        public LayerMask sightLayerMask;

        public override Brain Inject(Character body) => new BasicBrain(this, body, ID);
    }
}