using System;
using Sirenix.Utilities;
using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Characters.Modules;
using UnityEngine;

namespace Unbowed.SO {
    [GlobalConfig("Assets/Configs")]
    public class GlobalContext : GlobalConfig<GlobalContext> {
        public Action<Inventory> OpenOtherInventoryRequest;
        public Action<Inventory> CloseOtherInventoryRequest;
        
        public Character playerCharacter;
    }
}