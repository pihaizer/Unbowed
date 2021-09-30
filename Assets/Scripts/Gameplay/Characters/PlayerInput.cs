using Unbowed.Gameplay.Characters.Commands;
using Unbowed.SO;
using UnityEngine;
using UnityEngine.AI;

namespace Unbowed.Gameplay.Characters {
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerInput : MonoBehaviour {
        [SerializeField] MouseStateSO mouseStateSO;
        [SerializeField] LayerMask navMeshLayerMask;
        [SerializeField] float maxWalkDistance = 100f;

        PlayerCharacter _target;

        void Start() {
            _target = GetComponent<PlayerCharacter>();
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.R)) _target.Movement.ToggleRunning();
        }

        void FixedUpdate() {
            if (_target.Health.isDead || _target.areActionsBlocked) return;
            if (Input.GetMouseButton(0)) OnLMB();
        }

        void OnLMB() {
            if (mouseStateSO.isOffGameView) return;
            if (mouseStateSO.Target != null) {
                if (mouseStateSO.Target is IHittable hittable && hittable.CanBeHit()) {
                    if(!(_target.CurrentCharacterCommand is AttackCommand attackCommand) || attackCommand.Target != hittable)
                        _target.Execute(new AttackCommand(hittable));
                }
            } else {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100f,
                    navMeshLayerMask)) {
                    if (NavMesh.SamplePosition(hit.point, out var navMeshHit, maxWalkDistance, 1))
                        _target.Execute(new MoveCommand(navMeshHit.position));
                }
            }
        }

        [ContextMenu("Revive")]
        public void Revive() => _target.Health.Revive();
    }
}