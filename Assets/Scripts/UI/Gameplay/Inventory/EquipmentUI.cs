using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Items;

namespace Unbowed.UI.Gameplay.Inventory {
    public class EquipmentUI : SerializedMonoBehaviour {
        [OdinSerialize] private Dictionary<EquipmentSlot, EquipmentSlotUI> _equipments = Enum.GetValues(typeof(EquipmentSlot))
            .Cast<EquipmentSlot>().ToDictionary(slot => slot, slot => (EquipmentSlotUI) null);

        public Unbowed.Gameplay.Characters.Modules.Inventory Inventory { get; private set; }

        public void SetInventory(Unbowed.Gameplay.Characters.Modules.Inventory inventory) {
            Inventory = inventory;

            foreach (var equipment in _equipments) {
                if (equipment.Key == EquipmentSlot.None) continue;
                equipment.Value.Init(Inventory);
            }
        }
    }
}