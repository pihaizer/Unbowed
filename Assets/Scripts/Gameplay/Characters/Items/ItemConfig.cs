using Sirenix.OdinInspector;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items {
    [CreateAssetMenu]
    public class ItemConfig : SerializedScriptableObject {
        public string displayName;
        public Sprite icon;

        public bool isEquipment;
        [ShowIf(nameof(isEquipment))]
        public EquipmentConfig equipment;
    }
}