using System;
using System.Linq;
using Unbowed.Gameplay.Characters.Configs;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.SO;
using Unbowed.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

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

            int amount = Mathf.RoundToInt(config.amountCurve.Evaluate(Random.value));

            var itemLevelValidItems = ItemsContext.Instance.allItems
                .Where(i => i.itemLevel >= config.itemLevelCurve.Evaluate(0) &&
                            i.itemLevel <= config.itemLevelCurve.Evaluate(1)).ToArray();

            for (int i = 0; i < amount; i++) {
                int randomIndex = Random.Range(0, itemLevelValidItems.Length);
                var item = new Item(itemLevelValidItems[randomIndex], ItemLocation.InBag(_inventory, Vector2Int.zero));
                _inventory.Items.Add(item);
            }
        }

        void DropItems() {
            for (int i = _inventory.Items.Count - 1; i >= 0; i--) {
                Inventory.DropItem(_inventory.Items[i]);
            }
        }
    }
}