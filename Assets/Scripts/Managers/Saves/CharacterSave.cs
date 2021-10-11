using System;
using System.Collections.Generic;

using Sirenix.Serialization;

using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Items;

namespace Unbowed.Managers.Saves {
    [Serializable]
    public class CharacterSave {
        public List<Item> items;

        public static CharacterSave FromCharacter(Character character) {
            var save = new CharacterSave {
                items = new List<Item>(character.inventory.Items)
            };
            return save;
        }

        public void ApplyToCharacter(Character character) {
            if (items != null) character.inventory.SetItems(new List<Item>(items));
        }
    }
}