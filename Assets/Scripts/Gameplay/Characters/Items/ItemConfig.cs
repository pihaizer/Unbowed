﻿using System;
using DG.Tweening.Core.Easing;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.SO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Unbowed.Gameplay.Characters.Items {
    [CreateAssetMenu, InlineEditor]
    public class ItemConfig : SerializedScriptableObject {
        public string displayName;

        [MultiLineProperty(4)]
        public string description;

        [AssetsOnly, Required]
        public Sprite icon;

        [AssetsOnly, Required]
        public GameObject modelPrefab;

        [MinValue(1)]
        public Vector2Int size;

        public ItemType type;

        public float dropChanceWeight = 1f;

        [Range(1, 100)]
        [GUIColor("@Color.Lerp(Color.white, Color.green, itemLevel / 100f)")]
        public int itemLevel;

        [ShowIf(nameof(type), ItemType.Equipment)]
        [InlineProperty, HideLabel, BoxGroup]
        public EquipmentConfig equipment;

        [ShowIf(nameof(type), ItemType.Usable)]
        [InlineProperty, HideLabel, BoxGroup]
        public UsableItemConfig usableItem;

        [ShowIf(nameof(type), ItemType.Special)]
        public Color specialColor;

        public bool IsEquipment => type == ItemType.Equipment;
        public bool IsUsable => type == ItemType.Usable;
        public bool IsSpecial => type == ItemType.Special;

        void OnEnable() {
            if (ItemsConfig.Instance.allItems.Contains(this)) return;
            ItemsConfig.Instance.allItems.Add(this);

#if UNITY_EDITOR
            EditorUtility.SetDirty(ItemsConfig.Instance);
#endif
        }

        void OnDestroy() {
            if (!ItemsConfig.Instance.allItems.Contains(this)) return;
            ItemsConfig.Instance.allItems.Remove(this);

#if UNITY_EDITOR
            EditorUtility.SetDirty(ItemsConfig.Instance);
#endif
        }
    }
}