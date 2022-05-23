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
        private readonly PlayerBrainConfigSO _config;
        
        [Inject] private SignalBus _bus;
        [Inject] private PlayerController _playerController;
        [Inject] private ItemDragger _itemDragger;
        
        public PlayerBrain(PlayerBrainConfigSO config) : base(config.ID) {
            _config = config;
        }

        [Inject] 
        public void OnDependenciesInjected()
        {
            _bus.Subscribe<DroppedItemClickedSignal>(OnItemClicked);
        } 

        public override void SetBody(Character body)
        {
            base.SetBody(body);
            _playerController.SetPlayer(Body);
            ActivePlayer.SetPlayer(Body);
        }

        public override void OnDestroy() {
            if (ActivePlayer.Get() == Body) ActivePlayer.SetPlayer(null);
            _bus.Unsubscribe<DroppedItemClickedSignal>(OnItemClicked);
        }

        private void OnItemClicked(DroppedItemClickedSignal signal) {
            if (_itemDragger.IsDragging) return;
            if (!(Body.commands.MainCommand is InteractCommand interactCommand) ||
                interactCommand.Target != signal.Interactable) {
                Body.commands.Execute(new InteractCommand(signal.Interactable));
            }
        }

        public override void Update() {
            base.Update();
            if (Input.GetKeyDown(KeyCode.R)) Body.movement.ToggleRunning();
            if (Body.health.isDead || Body.areActionsBlocked) return;
            if (Input.GetMouseButton(0)) OnLMB();
        }

        private void OnLMB() {
            if (_itemDragger.IsDragging) return;
            if (MouseContext.Instance.GameViewTarget != null) {
                switch (MouseContext.Instance.GameViewTarget) {
                    case IHittable hittable
                        when hittable.CanBeHit() &&
                             (!(Body.commands.MainCommand is AttackCommand
                                  attackCommand) ||
                              attackCommand.Target != hittable):
                        Body.commands.Execute(new AttackCommand(hittable));
                        break;
                    case IInteractable interactable
                        when !(Body.commands.MainCommand is InteractCommand interactCommand) ||
                             interactCommand.Target != interactable:
                        Body.commands.Execute(new InteractCommand(interactable));
                        break;
                }
            } else {
                if (MouseContext.Instance.BlockedByUI) return;
                if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100f,
                    _config.navMeshLayerMask))
                    return;
                if (NavMesh.SamplePosition(hit.point, out var navMeshHit, _config.maxWalkDistance, 1))
                    Body.commands.Execute(new MoveCommand(navMeshHit.position));
            }
        }
    }
}