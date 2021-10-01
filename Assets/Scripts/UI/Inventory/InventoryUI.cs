using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.SO;
using Unbowed.UI;
using Unbowed.UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unbowed {
    public class InventoryUI : Menu {
        [SerializeField, Required, ChildGameObjectsOnly]
        BagsUI bagsUI;

        [SerializeField, Required, ChildGameObjectsOnly]
        EquipmentUI equipmentUI;

        public Inventory Inventory { get; private set; }

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

            void SlotOnEntered(BagSlotUI arg1, PointerEventData arg2) {
                _hoveredSlot = arg1;
            }

            void SlotOnExited(BagSlotUI arg1, PointerEventData arg2) {
                if (arg1 == _hoveredSlot) _hoveredSlot = null;
            }

            void SlotOnDragged(BagSlotUI from, PointerEventData data) {
                if (!_hoveredSlot) return;
                if (!Inventory.TryMoveItem(from.Item, _hoveredSlot.Location)) {
                    from.AnimateError();
                    _hoveredSlot.AnimateError();
                }
            }
        }
    }
}