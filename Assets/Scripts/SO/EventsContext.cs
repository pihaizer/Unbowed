using System;
using Sirenix.Utilities;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.Gameplay.Items;

namespace Unbowed.SO {
    [GlobalConfig("Assets/Configs")]
    public class EventsContext : GlobalConfig<EventsContext> {
        public Action<bool> showLoadingScreen;
        public Action<Inventory, bool> otherInventoryRequest;
        public Action<DroppedItem, bool> descriptionCreateRequest;
        public Action<DroppedItem, bool> descriptionShowRequest;
    }
}