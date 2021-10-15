using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.UI;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items {
    public partial class Item {
        public Inventory Inventory => location.inventory;
        public bool IsInBags => !location.isEquipped;
        public bool IsEquipped => location.isEquipped;

        // Properties
        public ItemConfig Config {
            get {
                if (_config == null) {
                    _config = ItemsConfig.Instance.allItems.Find(c => c.name == configName);
                }

                return _config;
            }
            set => _config = value;
        }

        public string Name => Config.displayName;
        
        public Color Color {
            get {
                if (IsEquipment()) return UIConfig.Instance.GetEquipmentColor(rarity);
                if (IsUsable) return Config.usableItem.color;

                return Config.specialColor;
            }
        }
        
        public bool IsUsable => Config && Config.IsUsable;
        
        public Vector2Int Size => Config.size;

        public bool IsEquipment() => Config.IsEquipment();
        public bool IsEquipment(out EquipmentConfig equipmentConfig) => Config.IsEquipment(out equipmentConfig);
    }
}