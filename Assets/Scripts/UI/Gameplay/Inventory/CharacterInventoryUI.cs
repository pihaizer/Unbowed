using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.SO;
using Unbowed.UI;
using Unbowed.UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unbowed {
    public class CharacterInventoryUI : Menu {
        [SerializeField, Required, ChildGameObjectsOnly]
        BagsUI bagsUI;

        [SerializeField, Required, ChildGameObjectsOnly]
        EquipmentUI equipmentUI;
        public Inventory Inventory { get; private set; }

        public void SetInventory(Inventory inventory) {
            Inventory = inventory;
            equipmentUI.SetInventory(Inventory);
            bagsUI.SetInventory(Inventory);
        }
    }
}