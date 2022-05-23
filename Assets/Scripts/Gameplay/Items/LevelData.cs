using System;
using Newtonsoft.Json;
using Unbowed.Gameplay.Characters.Modules;
using UnityEngine.Serialization;

namespace Unbowed.Gameplay.Characters.Items
{
    [Serializable, JsonObject]
    public class LevelData
    {
        public int Level = 1;
        public long Experience;

        public LevelData() {}

        public LevelData(ExperienceModule experience)
        {
            Level = experience.Level;
            Experience = experience.Experience;
        }
    }
}