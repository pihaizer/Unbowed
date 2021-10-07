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
            if (SceneDirector.Instance._allSceneConfigs.Contains(this)) return;
            SceneDirector.Instance._allSceneConfigs.Add(this);
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(SceneDirector.Instance);
#endif
        }

        void OnDestroy() {
            if (!SceneDirector.Instance._allSceneConfigs.Contains(this)) return;
            SceneDirector.Instance._allSceneConfigs.Remove(this);
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(SceneDirector.Instance);
#endif
        }
    }
}