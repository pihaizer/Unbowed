using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Items;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items
{
    public class UsableItem : Item
    {
        public new UsableItemConfig Config => _config is UsableItemConfig usableItemConfig ? usableItemConfig : null;

        public override Color Color => Config.color;

        public UsableItem(UsableItemConfig config) : base(config)
        {
        }
    }
}