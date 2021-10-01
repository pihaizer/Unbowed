using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unbowed.Gameplay.Characters.Modules {
    public class Inventory : SerializedMonoBehaviour {
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
            fromItem.Remove();
            SetItem(fromIndex, inventoryItems[toIndex]);
            SetItem(toIndex, fromItem);
        }

        public void SetEquipment(Item item) {
            if (item == null) return;
            if (item.TryEquip(this)) Changed?.Invoke();
        }

        public bool TryRemoveEquipment(Item item) {
            if (item == null) return false;
            if (item.Inventory != this || !item.IsInSlot) return false;
            item.Remove();
            return true;
        }

        public void SetItem(int index, Item item) {
            if (item == null) return;
            item.SetLocation(this, index);
            Changed?.Invoke();
        }

        public bool CanEquip(Item item, EquipmentSlot slot) => CanEquip(item) && item.config.equipment.slot == slot;

        public bool CanEquip(Item item) {
            return item != null && item.config.isEquipment;
        }

        public bool TryEquip(Item item) {
            if (!CanEquip(item)) {
                Debug.LogError("Tried to equip wrong item");
                return false;
            }

            Assert.IsNotNull(item);
            SetEquipment(item);
            return true;
        }
    }
}