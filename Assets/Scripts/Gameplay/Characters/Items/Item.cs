﻿using System;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.UI;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items {
    [Serializable, InlineProperty]
    public class Item {
        [Title("@config==null?\"Config not set\":Name")]
        public ItemConfig config;

        public ItemLocation location;

        [ShowIf(nameof(IsEquipment))]
        public EquipmentRarity rarity;

        public bool IsInBags => !location.isEquipped;
        public bool IsEquipped => location.isEquipped;
        public Inventory Inventory => location.inventory;

        public string Name => config.displayName;
        public Color Color => IsEquipment ? EquipmentColor : config.specialColor;
        public bool IsEquipment => config.IsEquipment;
        public EquipmentSlot Slot => config.IsEquipment ? config.equipment.slot : EquipmentSlot.None;
        public Vector2Int Size => config.size;
        public RectInt Rect => new RectInt(location.position, config.size);

        Color EquipmentColor => UIConfig.Instance.GetEquipmentColor(rarity);

        public Item(Item other) : this(other.config, other.location) => rarity = other.rarity;

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