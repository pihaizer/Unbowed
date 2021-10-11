using System;

using Unbowed.Gameplay.Characters.Configs;
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
            var items = _config.GenerateItems(data.killer ? data.killer.Stats["MagicFind"] : 0f);
            for (int i = items.Count - 1; i >= 0; i--) {
                Inventory.DropItem(items[i]);
                try {
                    Debug.Log(JsonUtility.ToJson(items[i].statModifiersContainer, true));
                    Debug.Log(JsonUtility.ToJson(items[i].statModifiersContainer.statModifiers[0], true));
                } catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
        }
    }
}