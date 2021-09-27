using Gameplay.Commands;
using SO.Brains;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Gameplay.AI.Brains {
    public class BasicBrain : Brain {
        readonly BasicBrainConfigSO _config;
        CharacterCommand _previousCharacterCommand;

        public BasicBrain(BasicBrainConfigSO config, Character body, int id) : base(body, id) {
            _config = config;
            body.StoppedExecuting += OnStoppedExecuting;
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
            if (body.isDead || body.areActionsBlocked) return;
            if (body.CurrentCharacterCommand == null) {
                SelectNewCommand();
            }
        }

        void OnStoppedExecuting(CharacterCommand characterCommand) {
            _previousCharacterCommand = characterCommand;
        }

        void SelectNewCommand() {
            if (!(_previousCharacterCommand is AttackCommand && !_previousCharacterCommand.Result) &&
                SeesWantedCharacter(out var wantedCharacter)) {
                Attack(wantedCharacter);
            } else if (_previousCharacterCommand is IdleCommand) {
                MoveToRandomPoint();
            } else {
                Idle(VectorRandom.Range(_config.idleAfterMoveTimeRange));
            }
        }

        bool SeesWantedCharacter(out Character character) {
            character = null;

            var hits = Physics.OverlapSphere(body.transform.position, _config.playerAggroRange);
            foreach (var hit in hits) {
                var hitCharacter = hit.GetComponentInParent<Character>();
                if (hitCharacter && IsWanted(hitCharacter) && Sees(hitCharacter) && CanGoTo(hitCharacter)) {
                    character = hitCharacter;
                    Debug.Log("Sees wanted character");
                    break;
                }
            }

            return character;
        }

        bool IsWanted(Character character) {
            return character != body && character.CanBeHit() && _config.targetTypes.Contains(character.characterType);
        }

        bool Sees(Character other) {
            var thisHead = body.transform.position + Vector3.up * (body.NavAgent.height + body.NavAgent.baseOffset);
            var otherHead = other.transform.position + Vector3.up * (other.NavAgent.height + other.NavAgent.baseOffset);
            bool sees = !Physics.Raycast(thisHead, otherHead - thisHead,
                Vector3.Distance(thisHead, otherHead), _config.sightLayerMask);
            Debug.DrawLine(thisHead, otherHead, sees ? Color.green : Color.red);
            return sees;
        }

        bool CanGoTo(Character character) {
            var path = new NavMeshPath();
            
            if (NavMesh.CalculatePath(body.transform.position,
                character.transform.position, NavMesh.AllAreas, path)) {
                return path.GetLength() < character.attackConfig.maxChaseRange;
            }

            return false;
        }

        void MoveToRandomPoint() {
            for (int i = 0; i < 10; i++) {
                var direction2d = Random.insideUnitCircle.normalized * VectorRandom.Range(_config.newPointsRadiusRange);
                var direction = new Vector3(direction2d.x, 0, direction2d.y);
                var newPoint = body.transform.position + direction;
                NavMesh.SamplePosition(newPoint, out var hit, _config.maxWalkDistance, NavMesh.AllAreas);
                var movePosition = hit.position;
                if (!IsMovePositionValid(movePosition)) continue;
                MoveToPoint(movePosition, _config.maxWalkTime);
                return;
            }

            if (restrictedZone) {
                var point = restrictedZone.GetClosestPoint(body.transform.position);
                NavMesh.SamplePosition(point, out var hit, _config.maxWalkDistance, NavMesh.AllAreas);
                MoveToPoint(hit.position);
            }
        }
    }
}