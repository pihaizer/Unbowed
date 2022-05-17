using Sirenix.Utilities;
using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.SO;
using Unbowed.UI;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Unbowed.Configs
{
    [CreateAssetMenu]
    public class GlobalConfig : ScriptableObject, IInitializable
    {
        public AllItemsConfig AllItemsConfig;
        public AllStatModifiers AllStatModifiers;
        public ScenesConfig ScenesConfig;
        public UIConfig UIConfig;
        
        public static GlobalConfig Instance
        {
            get
            {
#if UNITY_EDITOR
                if (_instance == null && !Application.isPlaying)
                {
                    _instance = FindConfig();
                }
#endif
                return _instance;
            }
        }

        private static GlobalConfig _instance;

        public void Initialize()
        {
            _instance = this;
        }
#if UNITY_EDITOR
        private static GlobalConfig FindConfig()
        {
            string[] guids = AssetDatabase.FindAssets("t:GlobalConfig");
            if (guids.IsNullOrEmpty()) return null;
            var config = AssetDatabase.LoadAssetAtPath<GlobalConfig>(AssetDatabase.GUIDToAssetPath(guids[0]));
            return config;
        }
    }
#endif
}