using Newtonsoft.Json;
using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Characters.Stats;
using Unbowed.Gameplay.Items;
using Unbowed.UI;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items
{
    public abstract class Equipment : Item
    {
        public new EquipmentConfig Config => _config is EquipmentConfig equipmentConfig ? equipmentConfig : null;

        public StatEffectorsBundle Stats;

        public abstract string EquipmentTypeName { get; }
        
        public bool IsEquipped => location.IsEquipped;
        
        [JsonProperty]
        public EquipmentRarity Rarity { get; set; }

        public override Color Color => UIConfig.Instance.GetEquipmentColor(Rarity);

        public Equipment(ItemConfig config) : base(config)
        {
        }
    }
}