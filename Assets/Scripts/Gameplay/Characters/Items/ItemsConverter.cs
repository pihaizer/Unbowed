#nullable enable
using System;
using System.Linq;
using Newtonsoft.Json;
using Unbowed.Configs;
using Unbowed.Gameplay.Characters.Items.Configs;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items
{
    public class ItemsConverter : JsonConverter
    {
        private AllItemsConfig _allItemsConfig;

        public ItemsConverter(AllItemsConfig allItemsConfig)
        {
            _allItemsConfig = allItemsConfig;
        }

        public override bool CanWrite => false;
        public override bool CanRead => false;

        public override bool CanConvert(Type objectType) => objectType == typeof(Item) ||
                                                            objectType.IsSubclassOf(typeof(Item));

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            return null;
        }
    }
}