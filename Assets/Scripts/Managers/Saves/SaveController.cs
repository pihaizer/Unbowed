using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using HyperCore.Utility;
using Newtonsoft.Json;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.Gameplay.Characters.Items.Configs;
using UnityEngine;
using Zenject;

namespace Unbowed.Managers.Saves
{
    public class SaveController
    {
        [Inject] private AllItemsConfig _allItemsConfig;

        public void Save(SaveFile saveFile)
        {
            string destination = Application.persistentDataPath + "/save.json";
            File.WriteAllText(destination, JsonConvert.SerializeObject(saveFile,
                new JsonSerializerSettings {ContractResolver = AddJsonTypenameContractResolver.Instance}));
        }

        public SaveFile Load()
        {
            string destination = Application.persistentDataPath + "/save.json";
            
            if (!File.Exists(destination)) return new SaveFile();
            
            string json = File.ReadAllText(destination);
            var file = JsonConvert.DeserializeObject<SaveFile>(json,
                new JsonSerializerSettings
                {
                    ContractResolver = AddJsonTypenameContractResolver.Instance,
                    Converters = {new ItemsConverter(_allItemsConfig)}
                });
            return file;
        }
    }
}