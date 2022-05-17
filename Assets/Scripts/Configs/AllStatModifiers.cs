using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Stats.Configs;
using Unbowed.Utility;
using UnityEditor;
using UnityEngine;

namespace Unbowed.Configs
{
    public class AllStatModifiers : ScriptableObject
    {
        public List<StatModifierConfig> statModifierConfigs;

        public static AllStatModifiers Instance => GlobalConfig.Instance.AllStatModifiers;

#if UNITY_EDITOR
        [Button]
        private void Collect()
        {
            statModifierConfigs = EditorAssetsUtility.FindAssetsOfType<StatModifierConfig>();
        }
#endif
    }
}