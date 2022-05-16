using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using Unbowed.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Unbowed.SO {
    public class ScenesConfig : ScriptableObject {
        public List<SceneConfig> _allSceneConfigs = new();

        public List<SceneConfig> SceneConfigs => _allSceneConfigs;
    }

    public class SceneChangeRequest {
        public readonly SceneConfig SceneConfig;
        public bool UseLoadingScreen;
        public bool UnloadOther;
        public bool SetActive;

        public SceneChangeRequest(SceneConfig config) {
            SceneConfig = config;
        }
    }
}