using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine.Serialization;
using Item = Unbowed.Gameplay.Characters.Items.Item;

namespace Unbowed.Managers.Saves
{
    [Serializable]
    public class CharacterSave
    {
        [JsonProperty] public List<Item> Items;
        [JsonProperty] public LevelData LevelData;

        public static CharacterSave FromCharacter(Character character)
        {
            var save = new CharacterSave
            {
                Items = new List<Item>(character.inventory.Items),
                LevelData = new LevelData(character.Experience)
            };
            return save;
        }

        public void ApplyToCharacter(Character character)
        {
            if (Items != null) character.inventory.SetItems(new List<Item>(Items));
            if (LevelData != null)
            {
                character.Experience.Level.Set(LevelData.Level);
                character.Experience.Experience.Set(LevelData.Experience);
            }
        }
    }
}