using System;

using Sirenix.OdinInspector;

using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Configs.Stats.Configs;
using Unbowed.Utility;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Stats.Configs {
    public class StatModifierConfig : SerializedScriptableObject {
        public StatType stat;
        
        public StatModifierType type;
        
        public bool isInteger = true;
        
        [ShowIf(nameof(isInteger))]
        public Vector2Int integerValueRange;
        
        [HideIf(nameof(isInteger))]
        public Vector2 floatValueRange;
        
        [HideIf(nameof(isInteger))]
        public int roundToDigit = 0;
        
        public Vector2Int itemLevelRange;

        public StatEffector Get(bool isPrimary = false) => new StatEffector {
            type = type,
            StatType = stat,
            value = isInteger
                ? VectorRandom.Range(integerValueRange)
                : (float) Math.Round(VectorRandom.Range(floatValueRange), roundToDigit),
            isPrimary = isPrimary
        };

        void OnEnable() {
            if (AllStatModifiers.Instance.statModifierConfigs.Contains(this)) return;
            AllStatModifiers.Instance.statModifierConfigs.Add(this);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(AllStatModifiers.Instance);
#endif
        }

        void OnDestroy() {
            AllStatModifiers.Instance.statModifierConfigs.Remove(this);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(AllStatModifiers.Instance);
#endif
        }
    }
}