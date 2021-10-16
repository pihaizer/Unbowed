using System;

using Unbowed.Gameplay.Characters.Configs;
using Unbowed.Gameplay.Characters.Stats;
using Unbowed.Gameplay.Characters.Stats.Configs;
using Unbowed.Gameplay.Items;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Modules {
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Inventory))]
    public class DropsModule : MonoBehaviour {
        Health _health;
        DropsConfig _config;

        public void Init(DropsConfig config) {
            _config = config;
            _health = GetComponent<Health>();
            _health.Died += DropItems;
        }

        void DropItems(DeathData data) {
            if (!_config.hasDrops) return;
            var items = _config.GenerateItems(data.killer ? data.killer.Stats[StatType.MagicFind] : 0f);
            for (int i = items.Count - 1; i >= 0; i--) {
                Inventory.DropItem(items[i]);
            }
        }
    }
}