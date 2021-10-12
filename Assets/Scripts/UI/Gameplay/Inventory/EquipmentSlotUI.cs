using Sirenix.OdinInspector;
using Unbowed.Gameplay.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unbowed.UI.Gameplay.Inventory {
    using Inventory = Unbowed.Gameplay.Characters.Modules.Inventory;

    public class EquipmentSlotUI : CellUI {
        [SerializeField, ChildGameObjectsOnly] ItemUI _itemUI;
        [SerializeField] EquipmentSlot _slot;

        Inventory _inventory;

        public void Init(Inventory inventory) {
            _inventory = inventory;
            
            SetItem(null);

            if (inventory.Items.Count > 0) {
                var equippedItem = inventory.Items.Find((item) => item.IsEquipped && item.location.slot == _slot);
                SetItem(equippedItem);
            }

            _itemUI.IsHoveredChanged += ItemUIOnIsHoveredChanged;
            _itemUI.Dragged += ItemUIOnDragged;
            _inventory.AddedItem += InventoryOnAddedItem;
            _inventory.RemovedItem += InventoryOnRemovedItem;
        }

        void ItemUIOnIsHoveredChanged(ItemUI itemUI, bool value, PointerEventData data) {
            if (value)
                OnPointerEnter();
            else
                OnPointerExit();
        }

        void ItemUIOnDragged(ItemUI itemUI, PointerEventData data) {
            OnPointerClick();
        }

        public override void SetItem(Item item) {
            base.SetItem(item);
            _itemUI.SetItem(Item);
            _itemUI.SetRaycastReceiverSize(GetComponent<RectTransform>().sizeDelta);
        }

        void InventoryOnAddedItem(Item item) {
            if (item.location.slot == _slot && item.IsEquipped) SetItem(item);
        }

        void InventoryOnRemovedItem(Item item) {
            if (item.location.slot == _slot && item.IsEquipped) SetItem(null);
        }

        void OnPointerEnter() {
            if (!_inventory) return;
            if (!ItemDragger.Instance.IsDragging) {
                if (Item != null) SetState(State.Hover);
                return;
            }

            var item = ItemDragger.Instance.Item;
            if (Inventory.CanEquipItem(item, _slot)) {
                SetState(State.Positive);
            } else {
                SetState(State.Error);
            }
        }

        void OnPointerClick() {
            if (!_inventory) return;
            if (ItemDragger.Instance.IsDragging) {
                var newItem = ItemDragger.Instance.Item;
                if (!_inventory.TryEquipItem(newItem, _slot, out var removedItem)) return;
                ItemDragger.Instance.DragItem(removedItem);
                _itemUI.OnPointerEnter(new PointerEventData(null));
            } else if (Item != null) {
                ItemDragger.Instance.DragItem(Item);
                _itemUI.OnPointerEnter(new PointerEventData(null));
            }
        }

        void OnPointerExit() {
            if (!_inventory) return;
            ResetState();
        }
    }
}