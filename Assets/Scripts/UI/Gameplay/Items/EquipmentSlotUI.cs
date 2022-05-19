using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.Gameplay.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Item = Unbowed.Gameplay.Characters.Items.Item;

namespace Unbowed.UI.Gameplay.Items
{
    public class EquipmentSlotUI : CellUI, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [ChildGameObjectsOnly]
        [SerializeField] private Image itemImage;
        [SerializeField] private EquipmentSlot _slot;
        [SerializeField] private ItemDescriptionCaller itemDescriptionCaller;

        [Inject] private ItemDragger _itemDragger;

        public EquipmentSlot Slot => _slot;

        private Inventory _inventory;
        private ItemDescriptionUI _itemDescriptionUI;
        private bool _isHovered;

        public void Init(Inventory inventory)
        {
            _inventory = inventory;

            SetItem(null);

            if (inventory.Items is {Count: > 0})
            {
                Item equippedItem = inventory.Items.Find(item =>
                    item is Equipment {IsEquipped: true} && item.location.slot == _slot);
                SetItem(equippedItem);
            }

            _inventory.AddedItem += InventoryOnAddedItem;
            _inventory.RemovedItem += InventoryOnRemovedItem;
        }

        protected override Color GetDefaultSlotColor() => UIConfig.Instance.defaultEquipmentSlotColor;

        public override void SetItem(Item item)
        {
            base.SetItem(item);
            itemDescriptionCaller.SetItem(item);
            itemImage.enabled = Item != null;
            itemImage.sprite = item?.Config.icon;
        }

        private void InventoryOnAddedItem(Item item)
        {
            if (item.location.slot == _slot) SetItem(item);
        }

        private void InventoryOnRemovedItem(Item item)
        {
            if (item.location.slot == _slot) SetItem(null);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovered = true;
            UpdateState();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovered = false;
            UpdateState();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_itemDragger.IsDragging)
            {
                Item newItem = _itemDragger.Item;
                if (!_inventory.TryEquipItem(newItem, _slot, out Item removedItem)) return;
                _itemDragger.DragItem(removedItem);
            }
            else if (Item != null) _itemDragger.DragItem(Item);
            UpdateState();
        }

        private void UpdateState()
        {
            if (_isHovered && _itemDragger.Item != null)
                SetState(_itemDragger.Item is Equipment equipment && Inventory.CanEquipItem(equipment, _slot) ? 
                    State.Positive : State.Error);
            else
                SetState(_isHovered ? State.Hover : State.Default);
        }
    }
}