using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Characters.AI.Brains;
using UnityEngine;
using Zenject;

namespace Unbowed.SO.Brains {
    [CreateAssetMenu(fileName = "BasicBrainConfig", menuName = "SO/Brains/BasicBrainConfig")]
    public class BasicBrainConfigSO : BrainConfigSO {
        public float playerAggroRange;
        public Vector2 newPointsRadiusRange = new Vector2(1, 5);
        public float maxWalkDistance = 10f;
        public float maxWalkTime = 999;
        public Vector2 idleAfterMoveTimeRange = new Vector2(1, 2);
        public List<CharacterTypeSO> targetTypes;
        public LayerMask sightLayerMask;

        public override Brain Create() => new BasicBrain(this);
    }
}