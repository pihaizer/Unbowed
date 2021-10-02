using System;
using Unbowed.Gameplay.Characters.Modules;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items {
    [Serializable]
    public class Item {
        public ItemConfig config;

        public ItemLocation location;

        public bool IsInBags => !location.isEquipped;
        public bool IsEquipped => location.isEquipped;
        public Inventory Inventory => location.inventory;
        public EquipmentSlot Slot => config.isEquipment ? config.equipment.slot : EquipmentSlot.None;

        public Item(ItemConfig config, ItemLocation location) {
            this.config = config;
            this.location = location;
        }
        
        public bool OverlapsWith(Item other) {
            if (location.isEquipped || other.location.isEquipped) return false;
            var otherRect = new RectInt(other.location.position, other.config.size);
            return OverlapsWith(otherRect);
        }
        
        public bool OverlapsWith(RectInt otherRect) {
            var thisRect = new RectInt(location.position, config.size);
            return thisRect.Overlaps(otherRect);
        }
    }
}