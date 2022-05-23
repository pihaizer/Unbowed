using System;
using Sirenix.OdinInspector;

namespace Unbowed.Gameplay.Characters.Configs
{
    [Serializable]
    public class ExperienceConfig
    {
        public int StartLevel;
        public long ExperienceOnKill;
        
        public bool CanLevelUp;
        [ShowIf(nameof(CanLevelUp))] 
        public float ExperienceGainDistance = 50f;
    }
}