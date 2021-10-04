using Unbowed.Gameplay.Characters.Commands;
using Unbowed.SO;
using Unbowed.UI;
using UnityEngine;
using UnityEngine.AI;

namespace Unbowed.Gameplay.Characters.Player {
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerInput : MonoBehaviour {
        [SerializeField] LayerMask navMeshLayerMask;
        [SerializeField] float maxWalkDistance = 100f;

        PlayerCharacter _target;

        void Start() {
            _target = GetComponent<PlayerCharacter>();
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.R)) _target.characterMovement.ToggleRunning();
            if (_target.health.isDead || _target.areActionsBlocked) return;
            if (Input.GetMouseButton(0)) OnLMB();
        }

        void OnLMB() {
            if (MouseState.Instance.BlockedByUI) return;
            if (ItemDragger.Instance.IsDragging) return;
            if (MouseState.Instance.Target != null) {
                if (MouseState.Instance.Target is IHittable hittable && hittable.CanBeHit()) {
                    if(!(_target.characterCommandExecutor.MainCommand is AttackCommand attackCommand) || attackCommand.Target != hittable)
                        _target.characterCommandExecutor.Execute(new AttackCommand(hittable));
                } else if (MouseState.Instance.Target is IInteractable interactable) {
                    _target.characterCommandExecutor.Execute(new InteractCommand(interactable));
                }
            } else {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100f,
                    navMeshLayerMask)) {
                    if (NavMesh.SamplePosition(hit.point, out var navMeshHit, maxWalkDistance, 1))
                        _target.characterCommandExecutor.Execute(new MoveCommand(navMeshHit.position));
                }
            }
        }

        [ContextMenu("Revive")]
        public void Revive() => _target.health.Revive();
    }
}