using Unbowed.Gameplay.Characters.Commands;
using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Signals;
using Unbowed.SO;
using Unbowed.SO.Brains;
using Unbowed.UI.Gameplay;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Unbowed.Gameplay.Characters.AI.Brains {
    public class PlayerBrain : Brain {
        private readonly Character _body;
        private readonly PlayerBrainConfigSO _config;
        private readonly SignalBus _bus;
        
        public PlayerBrain(PlayerBrainConfigSO config, Character body, SignalBus bus, int id) : base(body, id) {
            _body = body;
            _config = config;
            _bus = bus;
            _bus.Subscribe<DroppedItemClickedSignal>(OnItemClicked);
            ActivePlayer.SetPlayer(_body);
        }

        public override void OnDestroy() {
            if (ActivePlayer.Get() == _body) ActivePlayer.SetPlayer(null);
            _bus.Unsubscribe<DroppedItemClickedSignal>(OnItemClicked);
        }

        private void OnItemClicked(DroppedItemClickedSignal signal) {
            if (ItemDragger.Instance.IsDragging) return;
            if (!(_body.commands.MainCommand is InteractCommand interactCommand) ||
                interactCommand.Target != signal.Interactable) {
                _body.commands.Execute(new InteractCommand(signal.Interactable));
            }
        }

        public override void Update() {
            base.Update();
            if (Input.GetKeyDown(KeyCode.R)) _body.movement.ToggleRunning();
            if (_body.health.isDead || _body.areActionsBlocked) return;
            if (Input.GetMouseButton(0)) OnLMB();
        }

        private void OnLMB() {
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