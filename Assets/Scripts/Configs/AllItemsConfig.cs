using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Utility;
using UnityEngine;

namespace Unbowed.Configs
{
    public class AllItemsConfig : ScriptableObject
    {
        public List<ItemConfig> allItems;

        public static AllItemsConfig Instance => GlobalConfig.Instance.AllItemsConfig;
        

#if UNITY_EDITOR
        [Button]
        private void Collect()
        {
            allItems = EditorAssetsUtility.FindAssetsOfType<ItemConfig>();
        }
#endif
    }
}