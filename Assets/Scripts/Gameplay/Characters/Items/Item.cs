using System;
using System.Linq;

using Sirenix.OdinInspector;
using Sirenix.Serialization;

using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.UI;

using UnityEngine;
using UnityEngine.Serialization;

namespace Unbowed.Gameplay.Items {
    [Serializable, InlineProperty]
    public class Item {
        // Serialized fields
        [Title("@config==null?\"Config not set\":Name")]
        [Required]
        public ItemConfig config;

        public ItemLocation location;

        [ShowIf(nameof(IsEquipment))]
        public EquipmentRarity rarity;

        [FormerlySerializedAs("statsModifier")] public StatModifiersContainer statModifiersContainer;

        // Properties
        public Inventory Inventory => location.inventory;
        public bool IsInBags => !location.isEquipped;
        public bool IsEquipped => location.isEquipped;

        public string Name => config.displayName;
        public Color Color {
            get {
                if (IsEquipment) return UIConfig.Instance.GetEquipmentColor(rarity);
                if (IsUsable) return config.usableItem.color;

                return config.specialColor;
            }
        }

        public bool IsEquipment => config && config.IsEquipment;
        public bool IsUsable => config && config.IsUsable;
        public Vector2Int Size => config.size;


        // Constructors
        public Item(Item other) : this(other.config, other.location) {
            rarity = other.rarity;
        }

        public Item(ItemConfig config, ItemLocation location) {
            this.config = config;
            this.location = location;
        }
        
        // Utility methods
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