using System;
using Unbowed.Gameplay.Characters.Modules;

namespace Unbowed.Gameplay.Characters.Items {
    [Serializable]
    public class Item {
        public ItemConfig config;

        public ItemLocation location;

        public Item(ItemConfig config, ItemLocation location) {
            this.config = config;
            this.location = location;
        }
    }
}