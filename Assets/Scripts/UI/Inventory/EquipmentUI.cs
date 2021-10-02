using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Items;

namespace Unbowed.UI.Inventory {
    public class EquipmentUI : SerializedMonoBehaviour {
        [OdinSerialize] Dictionary<EquipmentSlot, ItemUI> equipments = Enum.GetValues(typeof(EquipmentSlot))
            .Cast<EquipmentSlot>().ToDictionary(slot => slot, slot => (ItemUI)null);

        public Gameplay.Characters.Modules.Inventory Inventory { get; private set; }

        public void SetInventory(Gameplay.Characters.Modules.Inventory inventory) {
            Inventory = inventory;

            foreach (var equipment in equipments) {
                if(equipment.Key == EquipmentSlot.None) continue;
                equipment.Value.SetItem(null);
            }

            UpdateEquipment();
        }

        void UpdateEquipment() {
            foreach (var equipment in equipments) {
                equipment.Value.SetItem(null);
            }

            foreach (var item in Inventory.Equipped) {
                equipments[item.location.slot].SetItem(item);
            }
        }
    }
}