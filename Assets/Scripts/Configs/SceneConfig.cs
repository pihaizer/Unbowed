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
    }
}