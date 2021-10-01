using UnityEngine;
using UnityEngine.AI;

namespace Unbowed.Gameplay.Characters.Commands {
    public class MoveCommand : CharacterCommand {
        readonly Vector3 _newMoveTarget;
        readonly float _maxTime;
        Character _character;
        float _startTime;

        public MoveCommand(Vector3 newMoveTarget, float maxTime = float.MaxValue) {
            _newMoveTarget = newMoveTarget;
            _maxTime = maxTime;
        }

        public override void Start(Character character) {
            _character = character;

            if (Vector3.Distance(_character.transform.position, _newMoveTarget) <
                _character.config.distances.noMoveRange) {
                Stop(true);
                return;
            }

            var path = new NavMeshPath();
            _character.characterMovement.NavAgent.CalculatePath(_newMoveTarget, path);

            if (path.status == NavMeshPathStatus.PathInvalid) {
                Stop(false);
                return;
            }

            _character.characterMovement.NavAgent.SetPath(path);
            _startTime = Time.time;
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
            if (_character.characterMovement.NavAgent.hasPath && _character.characterMovement.NavAgent.remainingDistance < 0.05f) {
                Stop(true);
            } else if (!_character.characterMovement.NavAgent.pathPending && !_character.characterMovement.NavAgent.hasPath ||
                       Time.time > _startTime + _maxTime) {
                Stop(false);
            }
        }

        public override void Stop(bool result) {
            base.Stop(result);
            _character.characterMovement.NavAgent.ResetPath();
        }

        public override string ToString() => $"Moving to {_newMoveTarget}";
    }
}