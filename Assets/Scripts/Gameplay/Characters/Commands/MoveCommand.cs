using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Commands {
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
                _character.movementConfig.noMoveRange) {
                Stop(true);
                return;
            }

            var path = new NavMeshPath();
            _character.NavAgent.CalculatePath(_newMoveTarget, path);

            if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid) {
                Stop(false);
                return;
            }

            _character.NavAgent.SetPath(path);
            _startTime = Time.time;
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
            if (_character.NavAgent.hasPath && _character.NavAgent.remainingDistance < 0.05f) {
                Stop(true);
            } else if (!_character.NavAgent.hasPath || Time.time > _startTime + _maxTime) {
                Stop(false);
            }
        }

        public override void Stop(bool result) {
            base.Stop(result);
            _character.NavAgent.ResetPath();
        }

        public override string ToString() => $"Moving to {_newMoveTarget}";
    }
}