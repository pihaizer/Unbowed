using Sirenix.OdinInspector;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items {
    [CreateAssetMenu, InlineEditor()]
    public class ItemConfig : SerializedScriptableObject {
        public string displayName;
        [MultiLineProperty(4)]
        public string description;
        public Sprite icon;
        
        [MinValue(1)] 
        public Vector2Int size;

        public ItemType type;

        [ShowIf(nameof(type), ItemType.Equipment)]
        public EquipmentConfig equipment;

        [ShowIf(nameof(type), ItemType.Special)]
        public Color specialColor;

        public bool IsEquipment => type == ItemType.Equipment;
        public bool IsUsable => type == ItemType.Usable;
        public bool IsSpecial => type == ItemType.Special;

        public Color Color => IsEquipment ? equipment.Color : specialColor;
    }
}