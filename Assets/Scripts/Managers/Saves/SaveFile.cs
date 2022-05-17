using System;
using System.Collections.Generic;

namespace Unbowed.Managers.Saves {
    [Serializable]
    public class SaveFile {
        public List<CharacterSave> characters = new();
    }
}