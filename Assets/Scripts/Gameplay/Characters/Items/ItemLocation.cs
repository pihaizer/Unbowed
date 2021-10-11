using System;

using Sirenix.OdinInspector;

using Unbowed.Gameplay.Characters.Modules;

using UnityEngine;

namespace Unbowed.Gameplay.Items {
    [Serializable]
    public struct ItemLocation {
        [NonSerialized]
        public Inventory inventory;

        public bool isEquipped;

        [ShowIf(nameof(isEquipped))]
        public EquipmentSlot slot;

        [HideIf(nameof(isEquipped))]
        public Vector2Int position;

        public static ItemLocation None => new ItemLocation(null);

        public static ItemLocation InBag(Inventory inventory, Vector2Int position) =>
            new ItemLocation(inventory, position);

        public static ItemLocation Equipped(Inventory inventory, EquipmentSlot slot) =>
            new ItemLocation(inventory) {isEquipped = true, slot = slot};

        ItemLocation(Inventory inventory, Vector2Int position) : this(inventory) {
            isEquipped = false;
            this.position = position;
        }

        ItemLocation(Inventory inventory) {
            this.inventory = inventory;
            position = -Vector2Int.one;
            isEquipped = false;
            slot = EquipmentSlot.None;
        }

        public override bool Equals(object obj) {
            if (!(obj is ItemLocation location)) return false;
            if (location.inventory != inventory) return false;
            if (location.isEquipped != isEquipped) return false;
            return location.isEquipped && location.slot == slot || location.position == position;
        }

        public static bool operator ==(ItemLocation il1, ItemLocation il2) {
            return il1.Equals(il2);
        }

        public static bool operator !=(ItemLocation il1, ItemLocation il2) {
            return !(il1 == il2);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = (inventory != null ? inventory.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ isEquipped.GetHashCode();
                hashCode = (hashCode * 397) ^ position.GetHashCode();
                hashCode = (hashCode * 397) ^ slot.GetHashCode();
                return hashCode;
            }
        }
    }
}