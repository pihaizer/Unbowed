using Unbowed.Gameplay.Items;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items.Configs
{
    [CreateAssetMenu]
    public class ShieldConfig : EquipmentConfig
    {
        public override bool Fits(EquipmentSlot slot) => 
            slot is EquipmentSlot.LeftHand or EquipmentSlot.RightHand;

        public override Item Generate(float value)
        {
            var shield = new Shield(this);
            GenerateStats(shield, value);
            return shield;
        }
    }
}