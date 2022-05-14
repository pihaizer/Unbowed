using Sirenix.OdinInspector;
using UnityEngine;

namespace Unbowed.UI.Gameplay.Inventory {
    public class CharacterInventoryUI : Menu {
        [SerializeField, Required, ChildGameObjectsOnly]
        private BagsUI bagsUI;

        [SerializeField, Required, ChildGameObjectsOnly]
        private EquipmentUI equipmentUI;

        private Unbowed.Gameplay.Characters.Modules.Inventory Inventory { get; set; }

        public void SetInventory(Unbowed.Gameplay.Characters.Modules.Inventory inventory) {
            Inventory = inventory;
            if (Inventory == null) return;
            equipmentUI.SetInventory(Inventory);
            bagsUI.SetInventory(Inventory);
        }
    }
}