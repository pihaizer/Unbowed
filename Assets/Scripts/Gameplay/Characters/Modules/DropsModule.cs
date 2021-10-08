﻿using Unbowed.Gameplay.Characters.Configs;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Modules {
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Inventory))]
    public class DropsModule : MonoBehaviour {
        Inventory _inventory;
        Health _health;

        public void Init(DropsConfig config) {
            _inventory = GetComponent<Inventory>();
            _health = GetComponent<Health>();
            GenerateItems(config);
            _health.Died += DropItems;
            _health.Revived += () => GenerateItems(config);
        }

        void GenerateItems(DropsConfig config) {
            if (!config.hasDrops) return;
            var items = config.GenerateItems();
            foreach (var item in items) {
                Inventory.SetLocation(item, ItemLocation.InBag(_inventory, Vector2Int.zero));
            }
        }

        void DropItems() {
            for (int i = _inventory.Items.Count - 1; i >= 0; i--) {
                Inventory.DropItem(_inventory.Items[i]);
            }
        }
    }
}