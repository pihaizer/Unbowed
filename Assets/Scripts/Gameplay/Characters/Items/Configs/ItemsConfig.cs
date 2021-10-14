using System;
using System.Collections.Generic;

using Sirenix.Utilities;

using Unbowed.Gameplay.Items;
using Unbowed.SO;
namespace Unbowed.Gameplay.Characters.Items.Configs {
    [GlobalConfig("Assets/Resources/Configs")]
    public class ItemsConfig : GlobalConfig<ItemsConfig> {
        public Action<IInteractable> droppedItemClicked;
        
        public DroppedItem droppedItemPrefab;

        public List<ItemConfig> allItems;
    }
}