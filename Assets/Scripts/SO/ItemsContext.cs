using System;
using Sirenix.Utilities;
using Unbowed.Gameplay;
using UnityEngine;

namespace Unbowed.SO {
    [GlobalConfig("Assets/Configs")]
    public class ItemsContext : GlobalConfig<ItemsContext> {
        public Action<IInteractable> droppedItemClicked;
    }
}