using System.Collections.Generic;
using System.Linq;
using Unbowed.Configs;
using Unbowed.Gameplay.Characters.Configs;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Characters.Stats;
using Unbowed.Gameplay.Characters.Stats.Configs;
using Unbowed.Gameplay.Items;

using UnityEngine;
using Zenject;

namespace Unbowed.Gameplay.Characters.Modules {
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Inventory))]
    public class Drops : MonoBehaviour {
        private Health _health;
        private DropsConfig _config;

        [Inject] private AllItemsConfig _allItemsConfig;
        [Inject] private ItemsDropper _itemsDropper;

        public void Init(DropsConfig config) {
            _config = config;
            _health = GetComponent<Health>();
            _health.Died += DropItems;
        }

        private void DropItems(DeathData data) {
            if (!_config.hasDrops) return;
            var items = GenerateItems(data.killer ? data.killer.Stats[StatType.MagicFind] : 0f);
            for (int i = items.Count - 1; i >= 0; i--) {
                _itemsDropper.DropItem(items[i], transform);
            }
        }

        private List<Item> GenerateItems(float bonusMagicFind) {
            var items = new List<Item>();
            if (!_config.hasDrops) return items;

            int amount = _config.amountWeights.Random();

            var itemLevelValidItems = _allItemsConfig.allItems
                .Where(i => i.itemLevel >= _config.equipmentLevelRange.x &&
                            i.itemLevel <= _config.equipmentLevelRange.y).ToArray();
            if (itemLevelValidItems.Length == 0) return items;

            for (int i = 0; i < amount; i++) {
                int randomIndex = Random.Range(0, itemLevelValidItems.Length);
                float randomValue = Random.value;
                float value = Mathf.Pow(randomValue, 1 / (_config.magicFind + bonusMagicFind));
                var item = itemLevelValidItems[randomIndex].Generate(value);
                items.Add(item);
            }

            foreach (var specialDrop in _config.specialDrops) {
                if (Random.value < specialDrop.chance) {
                    items.Add(specialDrop.item);
                }
            }

            return items;
        }
    }
}