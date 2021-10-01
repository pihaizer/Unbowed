using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unbowed.Gameplay.Characters.Modules {
    public class InventoryModule {
        public event Action Changed;

        [OdinSerialize]
        public Dictionary<EquipmentSlot, Item> equipments =
            new Dictionary<EquipmentSlot, Item>();

        public Item[] inventoryItems;

        public void Init(int size) {
            inventoryItems = new Item[size];
        }

        public void Swap(int fromIndex, int toIndex) {
            var fromItem = inventoryItems[fromIndex];
            SetItem(fromIndex, inventoryItems[toIndex]);
            SetItem(toIndex, fromItem);
        }

        public void SetItem(int index, Item item) {
            inventoryItems[index] = item;
            Changed?.Invoke();
        }

        public void SetEquipment(Item item) {
            equipments[item.config.equipment.slot] = item;
            Changed?.Invoke();
        }

        public bool CanEquip(int index) {
            return inventoryItems[index] != null && inventoryItems[index].config.isEquipment;
        }

        public bool TryEquip(int index) {
            if (!CanEquip(index)) {
                Debug.LogError("Tried to equip wrong item");
                return false;
            }

            var newEquipment = inventoryItems[index];
            var slot = newEquipment.config.equipment.slot;
            Assert.IsNotNull(newEquipment);
            var oldEquipment = equipments.ContainsKey(slot) ? equipments[slot] : null;
            SetEquipment(newEquipment);
            SetItem(index, oldEquipment);
            return true;
        }
    }
}