using Unbowed.Gameplay.Characters.Modules;

namespace Unbowed.Gameplay.Characters.Items {
    public struct ItemLocation {
        public Inventory inventory;
        public bool isEquipped;
        public int indexInBag;
        public EquipmentSlot slot;

        public ItemLocation(Inventory inventory, int indexInBag) : this(inventory){
            this.indexInBag = indexInBag;
        }

        public ItemLocation(Inventory inventory, EquipmentSlot slot) : this(inventory) {
            this.slot = slot;
            isEquipped = true;
        }

        ItemLocation(Inventory inventory) {
            this.inventory = inventory;
            indexInBag = -1;
            isEquipped = false;
            slot = EquipmentSlot.None;
        }

        public override bool Equals(object obj) {
            if (!(obj is ItemLocation location)) return false;
            if (location.inventory != inventory) return false;
            if (location.isEquipped != isEquipped) return false;
            return location.isEquipped ? location.slot == slot : location.indexInBag == indexInBag;
        }

        public static bool operator ==(ItemLocation il1, ItemLocation il2) {
            return il1.Equals(il2);
        }

        public static bool operator !=(ItemLocation il1, ItemLocation il2) {
            return !(il1 == il2);
        }
    }
}