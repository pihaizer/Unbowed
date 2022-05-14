using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unbowed.Gameplay.Characters.Stats.Configs;
using Unbowed.Gameplay.Items;
using UnityEditor;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items.Configs
{
    [GlobalConfig("Assets/Resources/Configs")]
    public class AllItemsConfig : ScriptableObject
    {
        public List<ItemConfig> allItems;

#if UNITY_EDITOR
        [Button]
        private void Collect()
        {
            allItems.Clear();
            string[] guids = AssetDatabase.FindAssets("t:ItemConfig");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var config = AssetDatabase.LoadAssetAtPath<ItemConfig>(path);
                if (config) allItems.Add(config);
            }
        }
#endif
    }
}