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

        InventoryModule _displayedInventory;

        void OnEnable() {
            if (!GlobalContext.Instance.playerCharacter ||
                !GlobalContext.Instance.playerCharacter.IsStarted) {
                return;
            }

            _displayedInventory = GlobalContext.Instance.playerCharacter.inventory;
            _displayedInventory.Changed += UpdateEquipment;

            foreach (var equipment in equipments) {
                equipment.Value.Init(0);
                equipment.Value.Clicked += OnEquipmentClicked;
            }

            UpdateEquipment();

            Debug.Log("Started bags UI");
        }

        void OnDisable() {
            if (_displayedInventory != null) _displayedInventory.Changed -= UpdateEquipment;
        }

        void OnEquipmentClicked(BagSlotUI arg1, PointerEventData arg2) {
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