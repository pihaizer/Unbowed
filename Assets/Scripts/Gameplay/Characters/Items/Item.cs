using System;
using System.Linq;

using Sirenix.OdinInspector;
using Sirenix.Serialization;

using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.Gameplay.Characters.Stats;
using Unbowed.UI;

using UnityEngine;
using UnityEngine.Serialization;

namespace Unbowed.Gameplay.Items {
    [Serializable, InlineProperty, LabelWidth(200)]
    public class Item : ISerializationCallbackReceiver {
        // Serialized fields
        [SerializeField, HideInInspector]
        string configName;

        public ItemLocation location;

        [ShowIf(nameof(IsEquipment))]
        public EquipmentRarity rarity;

        [FormerlySerializedAs("statModifiersContainer"),FormerlySerializedAs("statsModifier")]
        public StatEffectorsBundle statEffectorsBundle;

        // Properties
        public Inventory Inventory => location.inventory;
        public bool IsInBags => !location.isEquipped;
        public bool IsEquipped => location.isEquipped;

        public string Name => Config.displayName;
        public Color Color {
            get {
                if (IsEquipment) return UIConfig.Instance.GetEquipmentColor(rarity);
                if (IsUsable) return Config.usableItem.color;

                return Config.specialColor;
            }
        }

        public bool IsEquipment => Config && Config.IsEquipment;
        public bool IsUsable => Config && Config.IsUsable;
        public Vector2Int Size => Config.size;

        public ItemConfig Config {
            get {
                if (_config == null) {
                    _config = ItemsConfig.Instance.allItems.Find(c => c.name == configName);
                }

                return _config;
            }
            set => _config = value;
        }

        [ShowInInspector] ItemConfig _config;

        // Constructors
        public Item(Item other) : this(other.Config, other.location) {
            rarity = other.rarity;
        }

        public Item(ItemConfig config, ItemLocation location) {
            Config = config;
            this.location = location;
        }

        // Utility methods
        public bool OverlapsWith(Item other) {
            if (location.isEquipped || other.location.isEquipped) return false;
            var otherRect = new RectInt(other.location.position, other.Config.size);
            return OverlapsWith(otherRect);
        }

        public bool OverlapsWith(RectInt otherRect) {
            var thisRect = new RectInt(location.position, Config.size);
            return thisRect.Overlaps(otherRect);
        }

        public void OnBeforeSerialize() {
            configName = _config != null ? _config.name : null;
        }

        public void OnAfterDeserialize() { }
    }
}