using System;
using Sirenix.Utilities;
using Unbowed.Gameplay;
using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Modules;
using UnityEngine;

namespace Unbowed.SO {
    public static class ActivePlayer {
        public static event Action PlayerChanged;
        public static event Action<HealthChangeData> HealthChanged;
        public static event Action Died;
        public static event Action Revived;
        public static event Action StatsUpdated;

        static Character _player;

        public static bool Exists { get; private set; } = false;

        public static void SetPlayer(Character character) {
            if (_player == character) return;

            if (Exists) {
                _player.health.HealthChanged -= OnHealthChanged;
                _player.health.Died -= OnDied;
                _player.health.Revived -= OnRevived;
                _player.stats.Updated -= OnStatsUpdated;
            }

            _player = character;

            Exists = _player != null;

            if (Exists) {
                _player.health.HealthChanged += OnHealthChanged;
                _player.health.Died += OnDied;
                _player.health.Revived += OnRevived;
                _player.stats.Updated += OnStatsUpdated;
            }

            PlayerChanged?.Invoke();
        }

        public static Character Get() => Exists ? _player : null;

        public static Transform GetTransform() => Exists ? _player.transform : null;

        public static Health GetHealth() => Exists ? _player.health : null;
        
        public static Inventory GetInventory() => Exists ? _player.inventory : null;

        public static CharacterRuntimeStats GetStats() => Exists ? _player.stats : null;

        public static void Revive() {
            if (!Exists) return;
            _player.health.Revive();
            _player.movement.NavAgent.Warp(Vector3.zero);
            _player.transform.rotation = new Quaternion();
        }

        static void OnHealthChanged(HealthChangeData data) => HealthChanged?.Invoke(data);

        static void OnDied() => Died?.Invoke();

        static void OnRevived() => Revived?.Invoke();

        static void OnStatsUpdated() => StatsUpdated?.Invoke();
    }
}