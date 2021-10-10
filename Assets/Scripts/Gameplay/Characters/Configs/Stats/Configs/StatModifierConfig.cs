using Sirenix.OdinInspector;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs.Stats.Configs {
    public abstract class StatModifierConfig : SerializedScriptableObject {
        public StatType stat;
        public Vector2Int itemLevelRange;

        public abstract StatModifier Get();

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