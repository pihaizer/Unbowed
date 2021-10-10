using System;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs.Stats {
    [CreateAssetMenu(fileName = "Stat type", menuName = "Configs/Stats/Stat type")]
    public class StatType : ScriptableObject {
        public float defaultValue;
        
        void OnEnable() {
            if (AllStatTypes.Instance.statTypes.Contains(this)) return;
            AllStatTypes.Instance.statTypes.Add(this);
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(AllStatTypes.Instance);
#endif
        }

        void OnDestroy() {
            AllStatTypes.Instance.statTypes.Remove(this);
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(AllStatTypes.Instance);
#endif
        }
    }
}