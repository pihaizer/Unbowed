using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Unbowed.SO {
    [CreateAssetMenu(fileName = "Scene Config", menuName = "Configs/Scene Config", order = 0)]
    [ExecuteAlways]
    public class SceneConfig : ScriptableObject {
        public string sceneName;

        void OnEnable() {
            if (ScenesConfig.Instance._allSceneConfigs.Contains(this)) return;
            ScenesConfig.Instance._allSceneConfigs.Add(this);
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(ScenesConfig.Instance);
#endif
        }

        void OnDestroy() {
            if (!ScenesConfig.Instance._allSceneConfigs.Contains(this)) return;
            ScenesConfig.Instance._allSceneConfigs.Remove(this);
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(ScenesConfig.Instance);
#endif
        }
    }
}