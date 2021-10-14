using Sirenix.OdinInspector;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs.Stats.Configs {
    public class StatModifierConfig : SerializedScriptableObject {
        public StatType stat;
        public Vector2Int itemLevelRange;
        public Vector2 valueRange;
        public StatModifierType type;

        public StatModifier Get() => new StatModifier {
            type = type,
            StatType = stat,
            value = Random.Range(valueRange.x, valueRange.y)
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