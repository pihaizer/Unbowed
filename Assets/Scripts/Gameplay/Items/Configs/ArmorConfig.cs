using System.Collections.Generic;
using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Items.Enums;
using Unbowed.Gameplay.Characters.Stats;
using Unbowed.Gameplay.Items;

namespace Unbowed.Gameplay.Characters.Items.Configs
{
    public class ArmorConfig : EquipmentConfig
    {
        public ArmorType ArmorType;
        

        public override Item Generate(float value)
        {
            var armor = new Armor(this);
            GenerateStats(armor, value);
            return armor;
        }
        
        public override bool Fits(EquipmentSlot slot)
        {
            return ArmorType switch
            {
                ArmorType.Belt => slot is EquipmentSlot.Belt,
                ArmorType.Chest => slot is EquipmentSlot.Chest,
                ArmorType.Gloves => slot is EquipmentSlot.Hands,
                ArmorType.Helmet => slot is EquipmentSlot.Head,
                ArmorType.Boots => slot is EquipmentSlot.Feet,
                _ => false
            };
        }
    }
}