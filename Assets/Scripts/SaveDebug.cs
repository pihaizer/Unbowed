using System.Collections.Generic;
using HyperCore.Utility;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Unbowed.Configs;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Managers.Saves;
using UnityEngine;

namespace Unbowed
{
    public class SaveDebug : SerializedMonoBehaviour
    {
        [SerializeField] private List<Item> items;
        [SerializeField] private AllItemsConfig allItemsConfig;
        [SerializeField] private string stringToDeserialize;

        [Button] 
        public void Serialize()
        {
            var saveFile = new SaveFile
            {
                characters = new List<CharacterSave>
                {
                    new() {Items = items}
                }
            };
            Debug.Log(JsonConvert.SerializeObject(saveFile, new JsonSerializerSettings()
            {
                ContractResolver = new AddJsonTypenameContractResolver(),
                Converters = {new ItemsConverter(allItemsConfig)},
                Formatting = Formatting.Indented
            }));
        }

        [Button]
        public void Deserialize()
        {
            object saveFile = JsonConvert.DeserializeObject<SaveFile>(stringToDeserialize, new JsonSerializerSettings()
            {
                ContractResolver = new AddJsonTypenameContractResolver(),
                Converters = {new ItemsConverter(allItemsConfig)},
                Formatting = Formatting.Indented
            });
            Debug.Log(JsonConvert.SerializeObject(saveFile, new JsonSerializerSettings()
            {
                ContractResolver = new AddJsonTypenameContractResolver(),
                Converters = {new ItemsConverter(allItemsConfig)},
                Formatting = Formatting.Indented
            }));
        }
    }
}