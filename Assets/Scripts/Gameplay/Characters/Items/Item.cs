using System;
using System.Runtime.Serialization;
using HyperCore.Utility;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.Gameplay.Characters.Stats;
using Unbowed.Gameplay.Items;
using UnityEngine;
using Zenject;

namespace Unbowed.Gameplay.Characters.Items
{
    [Serializable, InlineProperty, LabelWidth(200), AddJsonTypename]
    public abstract class Item : ICloneable
    {
        [ShowInInspector]
        protected ItemConfig _config;

        public ItemLocation location = ItemLocation.None;

        public Inventory Inventory => location.Inventory;
        public bool IsInBags => !location.IsEquipped;

        public ItemConfig Config { get; set; }

        public virtual string Name => Config.displayName;

        public virtual Color Color => Color.white;

        public Vector2Int Size => Config.size;

        public Item(ItemConfig config) => Config = config;


        #region Serialization

        [SerializeField, HideInInspector]
        public string configName;

        #endregion

        #region Utility

        public bool OverlapsWith(Item other)
        {
            if (location.IsEquipped || other.location.IsEquipped) return false;
            var otherRect = new RectInt(other.location.position, other.Config.size);
            return OverlapsWith(otherRect);
        }

        public bool OverlapsWith(RectInt otherRect)
        {
            var thisRect = new RectInt(location.position, Config.size);
            return thisRect.Overlaps(otherRect);
        }

        #endregion

        public object Clone() => MemberwiseClone();
    }
}