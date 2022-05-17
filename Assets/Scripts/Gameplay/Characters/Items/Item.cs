using System;
using System.Linq;
using System.Runtime.Serialization;
using HyperCore.Utility;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Unbowed.Configs;
using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.Gameplay.Characters.Stats;
using Unbowed.Gameplay.Items;
using UnityEngine;
using Zenject;

namespace Unbowed.Gameplay.Characters.Items
{
    [JsonObject(MemberSerialization.OptIn), AddJsonTypename]
    [Serializable, InlineProperty, LabelWidth(200)]
    public abstract class Item : ICloneable
    {
        [ShowInInspector]
        protected ItemConfig _config;

        [JsonProperty]
        public ItemLocation location = ItemLocation.None;

        public Inventory Inventory => location.Inventory;
        public bool IsInBags => !location.IsEquipped;

        public ItemConfig Config
        {
            get => _config;
            set => _config = value;
        } 

        public virtual string Name => Config.displayName;

        public virtual Color Color => Color.white;

        public Vector2Int Size => Config.size;

        public Item(ItemConfig config) => _config = config;


        #region Serialization

        [JsonProperty]
        private string _configName;

        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            _configName = _config.name;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Config = AllItemsConfig.Instance.allItems.FirstOrDefault(config => config.name == _configName);
        }

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