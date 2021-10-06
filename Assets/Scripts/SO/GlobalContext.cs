using System;
using Sirenix.Utilities;
using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.Gameplay.Characters.Modules;
using UnityEngine;

namespace Unbowed.SO {
    [GlobalConfig("Assets/Configs")]
    public class GlobalContext : GlobalConfig<GlobalContext> {
        public Action<Inventory, bool> otherInventoryRequest;
        public Action<DroppedItem, bool> descriptionCreateRequest;
        public Action<DroppedItem, bool> descriptionShowRequest;
        
        public Character playerCharacter;
        public DroppedItem droppedItemPrefab;
    }
}