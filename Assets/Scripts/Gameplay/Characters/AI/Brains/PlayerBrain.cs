using Unbowed.Gameplay.Characters.Commands;
using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.SO;
using Unbowed.SO.Brains;
using Unbowed.UI.Gameplay;
using UnityEngine;
using UnityEngine.AI;

namespace Unbowed.Gameplay.Characters.AI.Brains {
    public class PlayerBrain : Brain {
        readonly Character _body;
        readonly PlayerBrainConfigSO _config;

        public PlayerBrain(PlayerBrainConfigSO config, Character body, int id) : base(body, id) {
            _body = body;
            _config = config;
            ItemsConfig.Instance.droppedItemClicked += OnItemClicked;
            ActivePlayer.SetPlayer(_body);
        }

        public override void OnDestroy() {
            ItemsConfig.Instance.droppedItemClicked -= OnItemClicked;
            if (ActivePlayer.Get() == _body) ActivePlayer.SetPlayer(null);
        }

        void OnItemClicked(IInteractable interactable) {
            if (ItemDragger.Instance.IsDragging) return;
            if (!(_body.commands.MainCommand is InteractCommand interactCommand) ||
                interactCommand.Target != interactable) {
                _body.commands.Execute(new InteractCommand(interactable));
            }
        }

        public override void Update() {
            base.Update();
            if (Input.GetKeyDown(KeyCode.R)) _body.movement.ToggleRunning();
            if (_body.health.isDead || _body.areActionsBlocked) return;
            if (Input.GetMouseButton(0)) OnLMB();
        }

        void OnLMB() {
            if (ItemDragger.Instance.IsDragging) return;
            if (MouseContext.Instance.GameViewTarget != null) {
                switch (MouseContext.Instance.GameViewTarget) {
                    case IHittable hittable
                        when hittable.CanBeHit() &&
                             (!(_body.commands.MainCommand is AttackCommand
                                  attackCommand) ||
                              attackCommand.Target != hittable):
                        _body.commands.Execute(new AttackCommand(hittable));
                        break;
                    case IInteractable interactable
                        when !(_body.commands.MainCommand is InteractCommand interactCommand) ||
                             interactCommand.Target != interactable:
                        _body.commands.Execute(new InteractCommand(interactable));
                        break;
                }
            } else {
                if (MouseContext.Instance.BlockedByUI) return;
                if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100f,
                    _config.navMeshLayerMask))
                    return;
                if (NavMesh.SamplePosition(hit.point, out var navMeshHit, _config.maxWalkDistance, 1))
                    _body.commands.Execute(new MoveCommand(navMeshHit.position));
            }
        }
    }
}