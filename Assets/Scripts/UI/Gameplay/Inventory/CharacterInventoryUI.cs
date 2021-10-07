using Sirenix.OdinInspector;
using UnityEngine;

namespace Unbowed.UI.Gameplay.Inventory {
    public class CharacterInventoryUI : Menu {
        [SerializeField, Required, ChildGameObjectsOnly]
        BagsUI bagsUI;

        [SerializeField, Required, ChildGameObjectsOnly]
        EquipmentUI equipmentUI;

        Unbowed.Gameplay.Characters.Modules.Inventory Inventory { get; set; }

        public void SetInventory(Unbowed.Gameplay.Characters.Modules.Inventory inventory) {
            Inventory = inventory;
            if (Inventory == null) return;
            equipmentUI.SetInventory(Inventory);
            bagsUI.SetInventory(Inventory);
        }
    }
}