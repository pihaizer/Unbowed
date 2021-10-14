using System;
using System.Collections.Generic;

using Sirenix.Utilities;

using Unbowed.Gameplay.Items;
using Unbowed.SO;

namespace Unbowed.Gameplay.Characters.Items.Configs {
    public class ItemsConfig : ConfigSingleton<ItemsConfig> {
        public Action<IInteractable> droppedItemClicked;
        
        public DroppedItem droppedItemPrefab;

        public List<ItemConfig> allItems;
    }
}