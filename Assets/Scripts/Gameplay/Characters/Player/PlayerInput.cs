using System;
using Unbowed.Gameplay.Characters.Commands;
using Unbowed.SO;
using Unbowed.UI;
using Unbowed.UI.Gameplay;
using UnityEngine;
using UnityEngine.AI;

namespace Unbowed.Gameplay.Characters.Player {
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerInput : MonoBehaviour {
        [SerializeField] LayerMask navMeshLayerMask;
        [SerializeField] float maxWalkDistance = 100f;

        PlayerCharacter _target;

        void Awake() {
            _target = GetComponent<PlayerCharacter>();
            ItemsContext.Instance.droppedItemClicked += OnItemClicked;
        }

        void OnDestroy() {
            ItemsContext.Instance.droppedItemClicked -= OnItemClicked;
        }

        void OnItemClicked(IInteractable interactable) {
            if (ItemDragger.Instance.IsDragging) return;
            if (!(_target.characterCommandExecutor.MainCommand is InteractCommand interactCommand) ||
                interactCommand.Target != interactable) {
                _target.characterCommandExecutor.Execute(new InteractCommand(interactable));
            }
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.R)) _target.movement.ToggleRunning();
            if (_target.health.isDead || _target.areActionsBlocked) return;
            if (Input.GetMouseButton(0)) OnLMB();
        }

        void OnLMB() {
            if (ItemDragger.Instance.IsDragging) return;
            if (MouseState.Instance.GameViewTarget != null) {
                switch (MouseState.Instance.GameViewTarget) {
                    case IHittable hittable
                        when hittable.CanBeHit() &&
                             (!(_target.characterCommandExecutor.MainCommand is AttackCommand
                                  attackCommand) ||
                              attackCommand.Target != hittable):
                        _target.characterCommandExecutor.Execute(new AttackCommand(hittable));
                        break;
                    case IInteractable interactable
                        when !(_target.characterCommandExecutor.MainCommand is InteractCommand interactCommand) ||
                             interactCommand.Target != interactable:
                        _target.characterCommandExecutor.Execute(new InteractCommand(interactable));
                        break;
                }
            } else {
                if (MouseState.Instance.BlockedByUI) return;
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