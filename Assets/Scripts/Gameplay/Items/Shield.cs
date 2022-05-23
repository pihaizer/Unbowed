using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Items;

namespace Unbowed.Gameplay.Characters.Items
{
    public class Shield : Equipment
    {
        public Shield(ItemConfig config) : base(config)
        {
        }

        public override string EquipmentTypeName => nameof(Shield);
    }
}