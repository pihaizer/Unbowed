using Unbowed.Gameplay;
using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.UI.Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Unbowed.UI.Gameplay
{
    public class PlayerController
    {
        [Inject] private SignalBus _bus;

        private static Character _player;

        public bool Exists => _player != null;
        
        public void SetPlayer(Character character) {
            if (_player == character) return;

            // if (Exists) {
            //     _player.health.HealthChanged -= OnHealthChanged;
            //     _player.health.Died -= OnDied;
            //     _player.health.Revived -= OnRevived;
            //     _player.Stats.Updated -= OnStatsUpdated;
            // }

            _player = character;

            // if (Exists) {
            //     _player.health.HealthChanged += OnHealthChanged;
            //     _player.health.Died += OnDied;
            //     _player.health.Revived += OnRevived;
            //     _player.Stats.Updated += OnStatsUpdated;
            // }

            _bus.Fire(new PlayerChangedSignal(_player));
        }
        
        public Character Get() => Exists ? _player : null;

        public Transform GetTransform() => Exists ? _player.transform : null;

        public Health GetHealth() => Exists ? _player.health : null;
        
        public Inventory GetInventory() => Exists ? _player.inventory : null;

        public Unbowed.Gameplay.Characters.Stats.Stats GetStats() => Exists ? _player.Stats : null;

        public void Revive() {
            if (!Exists) return;
            _player.health.Revive();
            _player.movement.NavAgent.Warp(Vector3.zero);
            _player.transform.rotation = new Quaternion();
        }

        // private void OnHealthChanged(HealthChangeData data) => _bus.Fire(new HealthChanged(data.target, data.source)))//HealthChanged?.Invoke(data);

        // private void OnDied(DeathData data) => Died?.Invoke(data);

        // private void OnRevived() => Revived?.Invoke();

        // private void OnStatsUpdated() => StatsUpdated?.Invoke();
    }
}