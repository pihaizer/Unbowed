using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.Gameplay.Items;
using UnityEngine;
using UnityEngine.EventSystems;

using Item = Unbowed.Gameplay.Characters.Items.Item;

namespace Unbowed.UI.Gameplay.Inventory {
    using Inventory = Unbowed.Gameplay.Characters.Modules.Inventory;

    public class EquipmentSlotUI : CellUI {
        [SerializeField, ChildGameObjectsOnly] private ItemUI _itemUI;
        [SerializeField] private EquipmentSlot _slot;

        public EquipmentSlot Slot => _slot;

        private Inventory _inventory;

        public void Init(Inventory inventory) {
            _inventory = inventory;
            
            SetItem(null);

            if (inventory.Items is {Count: > 0}) {
                Item equippedItem = inventory.Items.Find(item => item is Equipment {IsEquipped:true} && item.location.slot == _slot);
                SetItem(equippedItem);
            }

            _itemUI.IsHoveredChanged += ItemUIOnIsHoveredChanged;
            _itemUI.Dragged += ItemUIOnDragged;
            _inventory.AddedItem += InventoryOnAddedItem;
            _inventory.RemovedItem += InventoryOnRemovedItem;
        }

        protected override Color GetDefaultSlotColor() => UIConfig.Instance.defaultEquipmentSlotColor;

        private void ItemUIOnIsHoveredChanged(ItemUI itemUI, bool value, PointerEventData data) {
            if (value)
                OnPointerEnter();
            else
                OnPointerExit();
        }

        private void ItemUIOnDragged(ItemUI itemUI, PointerEventData data) {
            OnPointerClick();
        }

        public override void SetItem(Item item) {
            base.SetItem(item);
            _itemUI.SetItem(Item);
            _itemUI.SetRaycastReceiverSize(GetComponent<RectTransform>().sizeDelta);
        }

        private void InventoryOnAddedItem(Item item) {
            if (item.location.slot == _slot) SetItem(item);
        }

        private void InventoryOnRemovedItem(Item item) {
            if (item.location.slot == _slot) SetItem(null);
        }

        private void OnPointerEnter() {
            if (!_inventory) return;
            if (!ItemDragger.Instance.IsDragging) {
                if (Item != null) SetState(State.Hover);
                return;
            }

            Item item = ItemDragger.Instance.Item;
            if (item is Equipment equipment && Inventory.CanEquipItem(equipment, _slot)) {
                SetState(State.Positive);
            } else {
                SetState(State.Error);
            }
        }

        private void OnPointerClick() {
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

        private void OnPointerExit() {
            if (!_inventory) return;
            ResetState();
        }
    }
}