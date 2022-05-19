using Unbowed.Gameplay.Characters.Commands;
using Unbowed.SO.Brains;
using Unbowed.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace Unbowed.Gameplay.Characters.AI.Brains {
    public class BasicBrain : Brain
    {
        protected BasicBrainConfigSO Config;
        private Command _previousCommand;

        public BasicBrain(BasicBrainConfigSO config) : base(config.ID) {
            Config = config;
        }

        public override void SetBody(Character body)
        {
            base.SetBody(body);
            Body.commands.StoppedExecuting += OnStoppedExecuting;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if (Body.health.isDead || Body.areActionsBlocked) return;
            if (Body.commands.MainCommand == null) {
                SelectNewCommand();
            }
        }

        private void OnStoppedExecuting(Command characterCommand) {
            _previousCommand = characterCommand;
        }

        private void SelectNewCommand() {
            if (!(_previousCommand is AttackCommand && !_previousCommand.Result) &&
                SeesWantedCharacter(out var wantedCharacter)) {
                Attack(wantedCharacter);
            } else if (_previousCommand is IdleCommand) {
                MoveToRandomPoint();
            } else {
                Idle(VectorRandom.Range(Config.idleAfterMoveTimeRange));
            }
        }

        private bool SeesWantedCharacter(out Character character) {
            character = null;

            var hits = Physics.OverlapSphere(Body.transform.position, Config.playerAggroRange);
            foreach (var hit in hits) {
                var hitCharacter = hit.GetComponentInParent<Character>();
                if (hitCharacter && IsWanted(hitCharacter) && Sees(hitCharacter) && CanGoTo(hitCharacter)) {
                    character = hitCharacter;
                    break;
                }
            }

            return character;
        }

        private bool IsWanted(Character character) {
            return character != Body && character.CanBeHit() && Config.targetTypes.Contains(character.characterType);
        }

        private bool Sees(Character other) {
            var thisHead = Body.transform.position + Vector3.up * (Body.movement.NavAgent.height + Body.movement.NavAgent.baseOffset);
            var otherHead = other.transform.position + Vector3.up * (other.movement.NavAgent.height + other.movement.NavAgent.baseOffset);
            bool sees = !Physics.Raycast(thisHead, otherHead - thisHead,
                Vector3.Distance(thisHead, otherHead), Config.sightLayerMask);
            Debug.DrawLine(thisHead, otherHead, sees ? Color.green : Color.red);
            return sees;
        }

        private bool CanGoTo(Character character) {
            var path = new NavMeshPath();
            
            if (NavMesh.CalculatePath(Body.transform.position,
                character.transform.position, NavMesh.AllAreas, path)) {
                return path.GetLength() < character.config.distances.maxChaseRange;
            }

            return false;
        }

        private void MoveToRandomPoint() {
            for (int i = 0; i < 10; i++) {
                var direction2d = Random.insideUnitCircle.normalized * VectorRandom.Range(Config.newPointsRadiusRange);
                var direction = new Vector3(direction2d.x, 0, direction2d.y);
                var newPoint = Body.transform.position + direction;
                NavMesh.SamplePosition(newPoint, out var hit, Config.maxWalkDistance, NavMesh.AllAreas);
                var movePosition = hit.position;
                if (!IsMovePositionValid(movePosition)) continue;
                MoveToPoint(movePosition, Config.maxWalkTime);
                return;
            }

            if (RestrictedZone) {
                var point = RestrictedZone.GetClosestPoint(Body.transform.position);
                NavMesh.SamplePosition(point, out var hit, Config.maxWalkDistance, NavMesh.AllAreas);
                MoveToPoint(hit.position);
            }
        }
    }
}