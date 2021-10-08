using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unbowed.SO {
    [GlobalConfig("Assets/Resources/Configs")]
    public class ScenesConfig : GlobalConfig<ScenesConfig> {
        public List<SceneConfig> _allSceneConfigs = new List<SceneConfig>();
        
        public readonly List<SceneConfig> loadedScenes = new List<SceneConfig>();

        public void Init() {
            for (int i = 0; i < SceneManager.sceneCount; i++) {
                var scene = SceneManager.GetSceneAt(i);
                var config = _allSceneConfigs.Find(sceneConfig => sceneConfig.sceneName == scene.name);
                if (config != null) loadedScenes.Add(config);
            }
        }

        public void Load(SceneChangeRequest request) {
            if (loadedScenes.Contains(request.sceneConfig)) return;
            
            if (request.useLoadingScreen) {
                EventsContext.Instance.showLoadingScreen?.Invoke(true);
            }

            if (request.unloadOther) {
                for (int i = loadedScenes.Count - 1; i >= 0; i--) {
                    Unload(loadedScenes[i]);
                }
            }

            loadedScenes.Add(request.sceneConfig);

            SceneManager.LoadSceneAsync(request.sceneConfig.sceneName, LoadSceneMode.Additive).completed += operation => {
                if (request.setActive) {
                    var scene = SceneManager.GetSceneByName(request.sceneConfig.sceneName);
                    SceneManager.SetActiveScene(scene);
                }

                if (request.useLoadingScreen) {
                    EventsContext.Instance.showLoadingScreen?.Invoke(false);
                }
            };
        }

        public void Unload(SceneConfig sceneConfig) {
            if (!loadedScenes.Contains(sceneConfig)) return;

            loadedScenes.Remove(sceneConfig);
            SceneManager.UnloadSceneAsync(sceneConfig.sceneName);
        }
    }

    public class SceneChangeRequest {
        public SceneConfig sceneConfig;
        public bool useLoadingScreen;
        public bool unloadOther;
        public bool setActive;

        public SceneChangeRequest(SceneConfig config) {
            sceneConfig = config;
        } 
    }
}