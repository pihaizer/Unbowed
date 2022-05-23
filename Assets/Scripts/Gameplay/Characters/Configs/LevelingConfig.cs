using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Configs;
using Unbowed.Gameplay.Characters.Items;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unbowed.Gameplay.Characters.Configs
{
    [CreateAssetMenu]
    public class LevelingConfig : SerializedScriptableObject
    {
        [OdinSerialize, DictionaryDrawerSettings(IsReadOnly = true)]
        private Dictionary<int, long> _experienceDictionary;

        public int MaxLevel = 150;

#if UNITY_EDITOR
        [Button]
        private void FixLevelsNumber()
        {
            _experienceDictionary ??= new Dictionary<int, long>();
            for (int i = 1; i <= MaxLevel; i++) _experienceDictionary.TryAdd(i, i);
            foreach (int extraKey in _experienceDictionary.Keys.Where(key => key > MaxLevel))
            {
                _experienceDictionary.Remove(extraKey);
            }
            EditorUtility.SetDirty(this);
        }
#endif

        public long GetExperienceToLevelUp(int level)
        {
            return _experienceDictionary[level];
        }

        public static LevelingConfig Instance => GlobalConfig.Instance.LevelingConfig;
    }
}