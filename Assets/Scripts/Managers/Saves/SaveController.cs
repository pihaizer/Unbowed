using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using HyperCore.Utility;
using Newtonsoft.Json;
using Unbowed.Configs;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.Gameplay.Characters.Items.Configs;
using UnityEngine;
using Zenject;

namespace Unbowed.Managers.Saves
{
    public class SaveController : ISaveController
    {
        [Inject] private AllItemsConfig _allItemsConfig;

        public async Task<T> GetAsync<T>(string key)
        {
            string destination = Application.persistentDataPath + $"/{key}.json";

            if (!File.Exists(destination)) return default;

            string json = await File.ReadAllTextAsync(destination);
            var file = JsonConvert.DeserializeObject<T>(json,
                new JsonSerializerSettings
                {
                    ContractResolver = new AddJsonTypenameContractResolver(),
                    Converters = {new ItemsConverter(_allItemsConfig)},
                });
            return file;
        }

        public Task SetAsync<T>(string key, T value)
        {
            string destination = Application.persistentDataPath + $"/{key}.json";
            File.WriteAllText(destination, JsonConvert.SerializeObject(value,
                new JsonSerializerSettings
                {
                    ContractResolver = new AddJsonTypenameContractResolver(),
                    Converters = {new ItemsConverter(_allItemsConfig)}
                }));
            return Task.CompletedTask;
        }
    }
}