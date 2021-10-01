using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.SO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unbowed {
    public class EquipmentUI : SerializedMonoBehaviour {
        [OdinSerialize] Dictionary<EquipmentSlot, BagSlotUI> equipments = Enum.GetValues(typeof(EquipmentSlot))
            .Cast<EquipmentSlot>().ToDictionary(slot => slot, slot => (BagSlotUI)null);

        public List<BagSlotUI> Slots => equipments.Values.ToList();

        Inventory _displayedInventory;

        public void Init(InventoryUI parent) {
            _displayedInventory = parent.Inventory;
            _displayedInventory.Changed += UpdateEquipment;

            foreach (var equipment in equipments) {
                equipment.Value.Init(parent, equipment.Key);
            }

            UpdateEquipment();
        }

        void OnDisable() {
            if (_displayedInventory != null) _displayedInventory.Changed -= UpdateEquipment;
        }

        void UpdateEquipment() {
            foreach (var equipment in equipments) {
                equipment.Value.SetItem(_displayedInventory.equipments.ContainsKey(equipment.Key)
                    ? _displayedInventory.equipments[equipment.Key]
                    : null);
            }
        }
    }
}