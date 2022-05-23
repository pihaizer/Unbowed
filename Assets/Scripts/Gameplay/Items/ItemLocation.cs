using System;

using Sirenix.OdinInspector;

using Unbowed.Gameplay.Characters.Modules;

using UnityEngine;

namespace Unbowed.Gameplay.Items {
    [Serializable]
    public struct ItemLocation {
        [NonSerialized]
        public Inventory Inventory;

        public bool IsEquipped => slot != EquipmentSlot.None;

        public EquipmentSlot slot;

        [HideIf(nameof(IsEquipped))]
        public Vector2Int position;

        public static ItemLocation None => new(null);

        public static ItemLocation InBag(Inventory inventory, Vector2Int position) => 
            new(inventory) {slot = EquipmentSlot.None, position = position};

        public static ItemLocation Equipped(Inventory inventory, EquipmentSlot slot) => 
            new(inventory) {slot = slot};

        private ItemLocation(Inventory inventory) {
            Inventory = inventory;
            position = -Vector2Int.one;
            slot = EquipmentSlot.None;
        }

        public override bool Equals(object obj) {
            if (obj is not ItemLocation location) return false;
            if (location.Inventory != Inventory) return false;
            if (location.IsEquipped != IsEquipped) return false;
            return location.IsEquipped && location.slot == slot || location.position == position;
        }

        public static bool operator ==(ItemLocation il1, ItemLocation il2) {
            return il1.Equals(il2);
        }

        public static bool operator !=(ItemLocation il1, ItemLocation il2) {
            return !(il1 == il2);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = (Inventory != null ? Inventory.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsEquipped.GetHashCode();
                hashCode = (hashCode * 397) ^ position.GetHashCode();
                hashCode = (hashCode * 397) ^ slot.GetHashCode();
                return hashCode;
            }
        }
    }
}