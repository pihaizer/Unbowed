using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR

#endif

namespace Unbowed.Gameplay.Characters.Items.Configs {
    public abstract class ItemConfig : ScriptableObject {
        public string displayName;

        [MultiLineProperty(4)]
        public string description;

        [AssetsOnly]
        public Sprite icon;

        [AssetsOnly]
        public GameObject modelPrefab;

        [MinValue(1)]
        public Vector2Int size;

        [Min(1), GUIColor("@Color.Lerp(Color.white, Color.green, itemLevel / 100f)")]
        public int itemLevel;

        public abstract Item Generate(float value);

        private static List<ItemConfig> _allItemConfigs;
    }
}