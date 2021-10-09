using System.Collections.Generic;
using Unbowed.Gameplay.Items;
using Unbowed.Utility.Modifiers;

namespace Unbowed.Gameplay.Characters.Configs.Stats {
    public class ItemStatsModifier : BaseModifier {
        public override int Priority { get; } = 0;

        List<(StatType, Modifier<float>)> _modifiers = new List<(StatType, Modifier<float>)>();

        public ItemStatsModifier(Item item) {
            if (!item.IsEquipment) return;
            
        }

        public override void Apply(BaseModifiable baseModifiable) {
            
        }
    }
}