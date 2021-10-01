using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.SO;
using Unbowed.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unbowed {
    public class InventoryUI : Menu {
        [SerializeField, Required, ChildGameObjectsOnly]
        BagsUI bagsUI;

        [SerializeField, Required, ChildGameObjectsOnly]
        EquipmentUI equipmentUI;

        public Inventory Inventory { get; private set; }

        [ShowInInspector]
        BagSlotUI _hoveredSlot;

        protected override void Start() {
            base.Start();
            StartCoroutine(DelayedStart());
        }

        IEnumerator DelayedStart() {
            yield return new WaitUntil(() => GlobalContext.Instance.playerCharacter ||
                                             GlobalContext.Instance.playerCharacter.IsStarted);

            Inventory = GlobalContext.Instance.playerCharacter.inventory;

            equipmentUI.Init(this);
            bagsUI.Init(this);

            foreach (var slot in equipmentUI.Slots.Union(bagsUI.Slots)) {
                slot.Entered += SlotOnEntered;
                slot.Exited += SlotOnExited;
                slot.Dragged += SlotOnDragged;
            }
        }

        void SlotOnEntered(BagSlotUI arg1, PointerEventData arg2) {
            _hoveredSlot = arg1;
            // Debug.Log($"New hovered slot {arg1.Location} {arg1.Index}");
        }

        void SlotOnExited(BagSlotUI arg1, PointerEventData arg2) {
            if (arg1 == _hoveredSlot) _hoveredSlot = null;
        }

        void SlotOnDragged(BagSlotUI from, PointerEventData data) {
            if (_hoveredSlot == null || _hoveredSlot == from) return;
            if (from.Location == BagSlotUI.SlotLocation.Bags) {
                switch (_hoveredSlot.Location) {
                    case BagSlotUI.SlotLocation.Bags:
                        Debug.Log($"swapping {from.Index} {_hoveredSlot.Index}");
                        Inventory.Swap(from.Index, _hoveredSlot.Index);
                        break;
                    case BagSlotUI.SlotLocation.Equipment: {
                        if (!Inventory.CanEquip(from.Item, _hoveredSlot.EquipmentSlot)) {
                            from.AnimateError();
                            _hoveredSlot.AnimateError();
                            return;
                        }

                        var oldEquipment = _hoveredSlot.Item;
                        Inventory.TryEquip(from.Item);
                        if (oldEquipment != null) Inventory.SetItem(from.Index, oldEquipment);

                        break;
                    }
                }
            } else if (from.Location == BagSlotUI.SlotLocation.Equipment) {
                switch (_hoveredSlot.Location) {
                    case BagSlotUI.SlotLocation.Bags:
                        Debug.Log($"swapping {from.Index} {_hoveredSlot.Index}");
                        var item = from.Item;
                        if (!Inventory.TryRemoveEquipment(from.Item)) {
                            from.AnimateError();
                            _hoveredSlot.AnimateError();
                            return;
                        }

                        Inventory.SetItem(_hoveredSlot.Index, item);
                        break;
                    case BagSlotUI.SlotLocation.Equipment: {
                        var hoveredItem = _hoveredSlot.Item;
                        
                        if (hoveredItem != null && !Inventory.CanEquip(hoveredItem, from.EquipmentSlot)) {
                            from.AnimateError();
                            _hoveredSlot.AnimateError();
                            return;
                        }

                        var unequippedItem = from.Item;
                        Inventory.TryRemoveEquipment(from.Item);
                        Inventory.TryEquip(hoveredItem);
                        Inventory.SetItem(_hoveredSlot.Index, unequippedItem);

                        break;
                    }
                }
            }
        }
    }
}