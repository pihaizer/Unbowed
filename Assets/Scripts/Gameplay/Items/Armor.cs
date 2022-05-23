using Newtonsoft.Json;
using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Items;

namespace Unbowed.Gameplay.Characters.Items
{
    public class Armor : Equipment
    {
        [JsonIgnore]
        protected new ArmorConfig Config => _config is ArmorConfig armorConfig ? armorConfig : null; 
        
        public Armor(ItemConfig config) : base(config)
        {
        }

        public override string EquipmentTypeName => Config.ArmorType.ToString();
    }
}