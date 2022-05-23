using UnityEngine;
using UnityEngine.AI;

namespace Unbowed.Gameplay.Characters.Commands {
    public class MoveCommand : CharacterCommand {
        private readonly Vector3 _newMoveTarget;
        private readonly float _maxTime;
        private Character _character;
        private float _startTime;

        public MoveCommand(Vector3 newMoveTarget, float maxTime = float.MaxValue) {
            _newMoveTarget = newMoveTarget;
            _maxTime = maxTime;
        }

        public override void Start(Character character) {
            _character = character;

            if (Vector3.Distance(_character.transform.position, _newMoveTarget) <
                _character.Config.distances.noMoveRange) {
                Stop(true);
                return;
            }

            var path = new NavMeshPath();
            _character.movement.NavAgent.CalculatePath(_newMoveTarget, path);

            if (path.status == NavMeshPathStatus.PathInvalid) {
                Stop(false);
                return;
            }

            _character.movement.NavAgent.SetPath(path);
            _startTime = Time.time;
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
            if (_character.movement.NavAgent.hasPath && _character.movement.NavAgent.remainingDistance < 0.05f) {
                Stop(true);
            } else if (!_character.movement.NavAgent.pathPending && !_character.movement.NavAgent.hasPath ||
                       Time.time > _startTime + _maxTime) {
                Stop(false);
            }
        }

        public override void Stop(bool result) {
            base.Stop(result);
            _character.movement.NavAgent.ResetPath();
        }

        public override string ToString() => $"Moving to {_newMoveTarget}";
    }
}