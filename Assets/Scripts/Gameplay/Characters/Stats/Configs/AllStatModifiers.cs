using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unbowed.Gameplay.Characters.Stats.Configs;
using UnityEditor;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs.Stats.Configs
{
    [GlobalConfig("Assets/Resources/Configs")]
    public class AllStatModifiers : ScriptableObject
    {
        public List<StatModifierConfig> statModifierConfigs;

        #if UNITY_EDITOR
        [Button]
        private void Collect()
        {
            statModifierConfigs.Clear();
            string[] guids = AssetDatabase.FindAssets("t:StatModifierConfig");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var config = AssetDatabase.LoadAssetAtPath<StatModifierConfig>(path);
                if (config) statModifierConfigs.Add(config);
            }
        }
        #endif
    }
}