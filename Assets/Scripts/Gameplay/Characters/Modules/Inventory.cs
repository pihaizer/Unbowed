using System;
using System.Collections.Generic;
using System.Linq;

using Sirenix.OdinInspector;
using Sirenix.Serialization;

using Unbowed.Gameplay.Items;
using Unbowed.SO;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Modules {
    public class Inventory : SerializedMonoBehaviour {
        public event Action<Item> AddedItem;
        public event Action<Item> RemovedItem;

        [SerializeField] Vector2Int size;
        [OdinSerialize] List<Item> defaultItems;

        [ShowInInspector]
        public List<Item> Items { get; private set; }

        public List<Item> Equipped => Items.Where(it => it.location.isEquipped).ToList();
        public List<Item> InBags => Items.Where(it => !it.location.isEquipped).ToList();
        public Vector2Int Size => size;

        public void Init() => SetItems(defaultItems);

        public void SetItems(List<Item> items) {
            if (Items != null) {
                foreach (var item in Items) {
                    RemovedItem?.Invoke(item);
                    item.location = ItemLocation.None;
                }
            }

            Items = items ?? defaultItems;
            if (Items == null) return;

            foreach (var item in Items) {
                item.location.inventory = this;
                AddedItem?.Invoke(item);
            }
        }

        public bool TryAddItemToBags(Item item) {
            if (!CanAddToBags(item, out var position)) return false;
            SetLocation(item, ItemLocation.InBag(this, position));
            return true;
        }

        public bool TryMoveItemToLocation(Item item, ItemLocation location, out Item removedItem) {
            removedItem = null;
            if (!CanMoveItemToLocation(item, location, out removedItem)) return false;
            if (removedItem != null) SetLocation(removedItem, ItemLocation.None);
            SetLocation(item, location);
            return true;
        }

        public bool TryEquipItem(Item item, EquipmentSlot slot, out Item removedItem) {
            removedItem = null;
            if (!CanEquipItem(item, slot)) return false;
            removedItem = Items.Find(it => it.location.slot == slot);
            SetLocation(removedItem, ItemLocation.None);
            SetLocation(item, ItemLocation.Equipped(this, slot));

            if (item.config.equipment.type == EquipmentType.TwoHandedWeapon) {
                var leftHandItem = Items.Find(it => it.location.slot == EquipmentSlot.LeftHand);
                if (leftHandItem != null) {
                    if (!TryAddItemToBags(leftHandItem)) DropItem(leftHandItem);
                }
            }

            return true;
        }

        public bool CanMoveItemToLocation(Item item, ItemLocation location, out Item removedItem) {
            removedItem = null;
            if (location.isEquipped) return true;
            var itemRect = new RectInt(location.position, item.Size);
            if (!IsInGrid(itemRect)) return false;
            var overlaps = InBags
                .Where(other => other.OverlapsWith(new RectInt(location.position, item.config.size))).ToList();
            if (overlaps.Count > 1) return false;
            if (overlaps.Count == 1) {
                removedItem = overlaps.First();
            }

            return true;
        }

        public bool IsInGrid(RectInt rect) =>
            rect.x >= 0 && rect.y >= 0 && rect.x < Size.x - rect.width + 1 && rect.y < Size.y - rect.height + 1;

        public bool CanAddToBags(Item item, out Vector2Int position) {
            var bagItems = InBags;
            var itemRect = new RectInt(Vector2Int.zero, item.config.size);

            for (int i = 0; i < Size.x - itemRect.width + 1; i++) {
                for (int j = 0; j < Size.y - itemRect.height + 1; j++) {
                    itemRect.position = new Vector2Int(i, j);
                    if (!RectOverlapsWith(itemRect, bagItems)) continue;
                    position = itemRect.position;
                    return true;
                }
            }

            position = -Vector2Int.one;
            return false;
        }

        public static bool CanEquipItem(Item item, EquipmentSlot slot) =>
            item.IsEquipment && item.config.equipment.Fits(slot);

        public static void RemoveItem(Item item) => SetLocation(item, ItemLocation.None);

        public static void DropItem(Item item) {
            RemoveItem(item);
            var droppedItem = Instantiate(ItemsConfig.Instance.droppedItemPrefab);
            droppedItem.transform.position = ActivePlayer.GetTransform().position + Vector3.up;
            droppedItem.SetItem(item);
        }

        static void SetLocation(Item item, ItemLocation location) {
            if (item == null) return;

            if (item.Inventory != null) {
                item.Inventory.Items.Remove(item);
                item.Inventory.RemovedItem?.Invoke(item);
            }

            item.location = location;
            if (location.inventory == null) return;
            item.Inventory.Items.Add(item);
            item.Inventory.AddedItem?.Invoke(item);
        }

        static bool RectOverlapsWith(RectInt rect, IEnumerable<Item> items) {
            return items.All(it => !it.IsEquipped && !it.OverlapsWith(rect));
        }

        void AddItem(Item item) {
            if (item == null) return;
            Items.Add(item);
            item.location.inventory = this;
            AddedItem?.Invoke(item);
        }

#if UNITY_EDITOR
        [Button]
        void GiveItem(Item item) {
            TryAddItemToBags(new Item(item));
        }
#endif
    }
}