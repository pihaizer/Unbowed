using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace Unbowed.Gameplay.Characters.Commands {
    public class InteractCommand : CharacterCommand {
        public IInteractable Target { get; }
        private readonly float _maxTime;
        private Character _character;
        private float _startTime;


        public InteractCommand(IInteractable target, float maxTime = float.MaxValue) {
            Target = target;
            _maxTime = maxTime;
        }

        public override void Start(Character character) {
            _character = character;
            _startTime = Time.time;
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
            
            if (Target == null) {
                Stop(false);
                return;
            }

            if (TryInteract()) {
                Stop(true);
                return;
            }

            if (Time.time > _startTime + _maxTime) {
                Stop(false);
                return;
            }

            GoToTarget(Target);
        }

        public override void Stop(bool result) {
            _character.movement.Stop();
            base.Stop(result);
        }

        private void GoToTarget(IInteractable target) {
            var path = new NavMeshPath();
            var interactablePosition = target.GetTransform().position;

            _character.movement.NavAgent.CalculatePath(interactablePosition, path);

            if (path.status == NavMeshPathStatus.PathInvalid) {
                if (!NavMesh.Raycast(_character.transform.position,
                    interactablePosition, out var hit, NavMesh.AllAreas)) {
                    Debug.Log("Stopping interacting due to invalid path");
                    Stop(false);
                    return;
                }

                _character.movement.NavAgent.CalculatePath(hit.position, path);
                
                if (path.status == NavMeshPathStatus.PathInvalid) {
                    Debug.Log("Stopping interacting due to invalid path");
                    Stop(false);
                    return;
                }
            }

            _character.movement.NavAgent.SetPath(path);

            if (_character.movement.NavAgent.hasPath &&
                _character.movement.NavAgent.GetRemainingDistance() >
                _character.config.distances.maxChaseRange) {
                Debug.Log(
                    $"Stopping interacting due to remaining distance {_character.movement.NavAgent.GetRemainingDistance()}");
                Stop(false);
            }
        }

        private bool TryInteract() {
            if (!IsInRange(Target)) return false;

            _character.movement.NavAgent.ResetPath();
            Target.Interact(_character.gameObject);
            return true;
        }

        private bool IsInRange(IInteractable target) {
            if (target == null || target.GetTransform() == null) return false;

            return (target.GetTransform().position - _character.transform.position).sqrMagnitude <
                   Mathf.Pow(_character.config.distances.interactRange, 2);
        }

        public override string ToString() => $"Interacting with {Target.GetTransform().name}";
    }
}