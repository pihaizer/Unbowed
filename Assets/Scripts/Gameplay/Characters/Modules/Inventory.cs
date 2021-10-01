using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unbowed.Gameplay.Characters.Modules {
    public class Inventory : SerializedMonoBehaviour {
        public event Action Changed;

        [ShowInInspector]
        public List<Item> Items { get; private set; }
        public List<Item> Equipped => Items.Where(it => it.location.isEquipped).ToList();
        public List<Item> InBags => Items.Where(it => !it.location.isEquipped).ToList();
        public int Size { get; private set; }

        public void Init(int size, List<Item> savedItems = null) {
            Size = size;
            Items = savedItems == null ? new List<Item>() : this.Items;
        }

        public bool TryMoveItem(Item item, ItemLocation location) {
            if (item == null) return false;
            if (item.location.inventory != this) return false;
            if (item.location == location) return true;

            switch (item.location.isEquipped) {
                case false when location.isEquipped:
                    return TryEquipItem(item);
                case false when !location.isEquipped: {
                    var other = Items.Find(it => it.location == location);

                    if (other != null)
                        SwapLocations(item, other);
                    else {
                        SetLocation(item, location);
                    }

                    return true;
                }
                case true when location.isEquipped:
                    return false;
                case true when !location.isEquipped: {
                    var other = Items.Find(it => it.location == location);

                    if (other != null) {
                        return TryEquipItem(other);
                    }

                    SetLocation(item, location);
                    return true;
                }
                default:
                    throw new Exception("Impossible situation");
            }
        }

        public bool CanAddToInventory(Item item) {
            return InBags.Count < Size;
        }

        public bool TryAddItemToInventory(Item item) {
            var bagItems = InBags;
            if (bagItems.Count >= Size) return false;

            for (int i = 0; i < Size; i++) {
                if (bagItems.Exists(it => it.location.indexInBag == i)) continue;
                item.location = new ItemLocation(this, i);
                Items.Add(item);
                Changed?.Invoke();
                return true;
            }

            throw new Exception("Impossible situation");
        }

        public bool CanEquipItem(Item item) {
            if (item.location.inventory != this) return false;
            if (!item.config.isEquipment) return false;
            if (item.location.isEquipped) return false;
            return true;
        }

        public bool TryEquipItem(Item item) {
            if (!CanEquipItem(item)) return false;
            var slot = item.config.equipment.slot;
            var oldItem = Items.Find((it) => it.location.isEquipped && it.location.slot == slot);
            if (oldItem != null)
                SwapLocations(oldItem, item);
            else
                item.location = new ItemLocation(this, slot);
            Changed?.Invoke();
            return true;
        }

        void SetLocation(Item item, ItemLocation location) {
            if (item == null) return;
            item.location = location;
            Changed?.Invoke();
        }

        static void SwapLocations(Item from, Item to) {
            if (from == null || to == null) return;
            if (from.location == to.location) return;
            var tempLocation = from.location;
            from.location = to.location;
            to.location = tempLocation;
        }
    }
}