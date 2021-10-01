using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Items;

namespace Unbowed.UI.Inventory {
    public class EquipmentUI : SerializedMonoBehaviour {
        [OdinSerialize] Dictionary<EquipmentSlot, BagSlotUI> equipments = Enum.GetValues(typeof(EquipmentSlot))
            .Cast<EquipmentSlot>().ToDictionary(slot => slot, slot => (BagSlotUI)null);

        public List<BagSlotUI> Slots => equipments.Values.ToList();

        Gameplay.Characters.Modules.Inventory _inventory;

        public void Init(InventoryUI parent) {
            _inventory = parent.Inventory;
            _inventory.Changed += UpdateEquipment;

            foreach (var equipment in equipments) {
                equipment.Value.Init(new ItemLocation(_inventory, equipment.Key));
            }

            UpdateEquipment();
        }

        void OnDisable() {
            if (_inventory != null) _inventory.Changed -= UpdateEquipment;
        }

        void UpdateEquipment() {
            foreach (var equipment in equipments) {
                equipment.Value.SetItem(null);
            }

            foreach (var item in _inventory.Equipped) {
                equipments[item.location.slot].SetItem(item);
            }
        }
    }
}