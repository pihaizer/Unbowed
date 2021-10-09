using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using Unbowed.Gameplay;
using Unbowed.Gameplay.Items;

namespace Unbowed.SO {
    [GlobalConfig("Assets/Resources/Configs")]
    public class ItemsConfig : GlobalConfig<ItemsConfig> {
        public Action<IInteractable> droppedItemClicked;
        
        public DroppedItem droppedItemPrefab;

        public List<ItemConfig> allItems;
    }
}