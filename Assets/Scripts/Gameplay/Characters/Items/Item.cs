using System;

using Sirenix.OdinInspector;

using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Characters.Stats;
using Unbowed.Gameplay.Items;

namespace Unbowed.Gameplay.Characters.Items {
    [Serializable, InlineProperty, LabelWidth(200)]
    public partial class Item {
        [ShowInInspector] 
        ItemConfig _config;
        
        public ItemLocation location;

        [ShowIf(nameof(IsEquipment))]
        public EquipmentRarity rarity;

        public StatEffectorsBundle statEffectorsBundle;

        public Item(Item other) : this(other.Config, other.location) {
            rarity = other.rarity;
        }

        public Item(ItemConfig config, ItemLocation location) {
            Config = config;
            this.location = location;
        }
    }
}