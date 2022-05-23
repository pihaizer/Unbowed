using Unbowed.Gameplay.Characters.Items.Enums;
using Unbowed.Gameplay.Items;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items.Configs
{
    [CreateAssetMenu]
    public class MeleeWeaponConfig : WeaponConfig
    {
        public MeleeWeaponType MeleeWeaponType;
        
        public override Item Generate(float value)
        {
            var item = new MeleeWeapon(this);
            GenerateStats(item, value);
            return item;
        }
    }
}