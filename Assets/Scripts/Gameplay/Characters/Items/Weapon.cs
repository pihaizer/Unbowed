using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Items;

namespace Unbowed.Gameplay.Characters.Items
{
    public abstract class Weapon : Equipment
    {
        public new WeaponConfig Config => _config is WeaponConfig weaponConfig ? weaponConfig : null;

        public Weapon(ItemConfig config) : base(config)
        {
        }
    }
}