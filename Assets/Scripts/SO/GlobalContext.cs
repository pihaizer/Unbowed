using Sirenix.Utilities;
using Unbowed.Gameplay.Characters;
using UnityEngine;

namespace Unbowed.SO {
    [GlobalConfig("Assets/Configs")]
    public class GlobalContext : GlobalConfig<GlobalContext> {
        public Character playerCharacter;
    }
}