using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

using Unbowed.Gameplay.Items;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items.Configs {
    [GlobalConfig("Assets/Resources/Configs")]
    public class ItemsConfig : GlobalConfig<ItemsConfig> {
        public Action<IInteractable> droppedItemClicked;

        [AssetsOnly, Required]
        public GameObject defaultItemModelPrefab;
        
        public DroppedItem droppedItemPrefab;

        public List<ItemConfig> allItems;
    }
}