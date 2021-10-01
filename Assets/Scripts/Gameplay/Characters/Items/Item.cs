using Unbowed.Gameplay.Characters.Modules;

namespace Unbowed.Gameplay.Characters.Items {
    public class Item {
        public ItemConfig config;

        public Inventory Inventory { get; private set; }
        public bool IsInBags { get; private set; }
        public int BagsIndex { get; private set; }
        public bool IsInSlot { get; private set; }
        public EquipmentSlot LocationSlot { get; private set; }

        public void SetLocation(Inventory inventory, int inventoryIndex) {
            if (inventory == Inventory && IsInBags && BagsIndex == inventoryIndex) return;
            Remove();

            Inventory = inventory;
            IsInBags = true;
            BagsIndex = inventoryIndex;
            Inventory.inventoryItems[inventoryIndex] = this;
        }

        public bool TryEquip(Inventory inventory) {
            if (inventory == Inventory && IsInSlot && LocationSlot == config.equipment.slot) return true;
            Remove();

            Inventory = inventory;
            IsInBags = true;
            LocationSlot = config.equipment.slot;
            Inventory.equipments[LocationSlot] = this;
            return true;
        }

        public void Remove() {
            if (!Inventory) return;
            if (IsInBags) {
                Inventory.inventoryItems[BagsIndex] = null;
                IsInBags = false;
            } else if (IsInSlot) {
                Inventory.equipments.Remove(LocationSlot);
                IsInSlot = false;
            }
        }
    }
}