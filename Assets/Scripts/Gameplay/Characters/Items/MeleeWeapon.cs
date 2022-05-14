using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Items;

namespace Unbowed.Gameplay.Characters.Items
{
    public class MeleeWeapon : Weapon
    {
        protected new MeleeWeaponConfig Config => _config is MeleeWeaponConfig meleeWeaponConfig ? meleeWeaponConfig : null; 
        public MeleeWeapon(ItemConfig config) : base(config)
        {
        }

        public override string EquipmentTypeName => Config.MeleeWeaponType.ToString();
    }
}