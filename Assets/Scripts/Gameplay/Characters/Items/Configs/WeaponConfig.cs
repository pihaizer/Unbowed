using Unbowed.Gameplay.Items;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items.Configs
{
    public abstract class WeaponConfig : EquipmentConfig
    {
        [SerializeField] private bool isTwoHanded;

        public bool IsTwoHanded => isTwoHanded;

        public override bool Fits(EquipmentSlot slot)
        {
            return slot is EquipmentSlot.RightHand or EquipmentSlot.LeftHand;
        }
    }
}