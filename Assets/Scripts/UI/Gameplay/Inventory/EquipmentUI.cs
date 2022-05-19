using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Items;
using UnityEngine;

namespace Unbowed.UI.Gameplay.Inventory {
    public class EquipmentUI : MonoBehaviour {
        [SerializeField] private List<EquipmentPair> _equipments;

        public Unbowed.Gameplay.Characters.Modules.Inventory Inventory { get; private set; }

        public void SetInventory(Unbowed.Gameplay.Characters.Modules.Inventory inventory) {
            Inventory = inventory;

            foreach ((EquipmentSlot slot, EquipmentSlotUI ui) in _equipments) {
                if (slot == EquipmentSlot.None) continue;
                ui.Init(Inventory);
            }
        }

        [Button]
        private void Collect()
        {
            _equipments = new List<EquipmentPair>();
            
            foreach (EquipmentSlotUI equipmentSlotUI in GetComponentsInChildren<EquipmentSlotUI>())
            {
                _equipments.Add(new EquipmentPair(equipmentSlotUI.Slot, equipmentSlotUI));
            }
        }

        [Serializable]
        private struct EquipmentPair
        {
            [HorizontalGroup, HideLabel]
            public EquipmentSlot Slot;
            
            [HorizontalGroup, HideLabel]
            public EquipmentSlotUI Ui;

            public EquipmentPair(EquipmentSlot slot, EquipmentSlotUI ui)
            {
                Slot = slot;
                Ui = ui;
            }
            

            public void Deconstruct(out EquipmentSlot slot, out EquipmentSlotUI ui)
            {
                slot = Slot;
                ui = Ui;
            }
        }
    }
}