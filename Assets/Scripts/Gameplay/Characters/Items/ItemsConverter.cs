#nullable enable
using System;
using System.Linq;
using Newtonsoft.Json;
using Unbowed.Gameplay.Characters.Items.Configs;

namespace Unbowed.Gameplay.Characters.Items
{
    public class ItemsConverter : JsonConverter<Item>
    {
        private AllItemsConfig _allItemsConfig;

        public ItemsConverter(AllItemsConfig allItemsConfig)
        {
            _allItemsConfig = allItemsConfig;
        }

        public override void WriteJson(JsonWriter writer, Item? value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }

        public override Item? ReadJson(JsonReader reader, Type objectType, Item? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (existingValue == null) return null;
            if (existingValue.configName != null)
            {
                existingValue.Config =
                    _allItemsConfig.allItems.FirstOrDefault(config => config.name == existingValue.configName);
            }

            return existingValue;
        }
    }
}