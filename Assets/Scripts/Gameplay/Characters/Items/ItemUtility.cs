using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items {
    public partial class Item {
        public bool OverlapsWith(Characters.Items.Item other) {
            if (location.isEquipped || other.location.isEquipped) return false;
            var otherRect = new RectInt(other.location.position, other.Config.size);
            return OverlapsWith(otherRect);
        }

        public bool OverlapsWith(RectInt otherRect) {
            var thisRect = new RectInt(location.position, Config.size);
            return thisRect.Overlaps(otherRect);
        }
    }
}