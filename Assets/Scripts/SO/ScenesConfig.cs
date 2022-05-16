using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using Unbowed.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Unbowed.SO {
    [GlobalConfig("Assets/Resources/Configs")]
    public class ScenesConfig : GlobalConfig<ScenesConfig> {
        public List<SceneConfig> _allSceneConfigs = new();

        public readonly List<SceneConfig> LoadedScenes = new();

        [Inject] private SignalBus _bus;

        public void Init() {
            for (int i = 0; i < SceneManager.sceneCount; i++) {
                Scene scene = SceneManager.GetSceneAt(i);
                SceneConfig config = _allSceneConfigs.Find(sceneConfig => sceneConfig.sceneName == scene.name);
                if (config != null) LoadedScenes.Add(config);
            }
        }

        public AsyncOperation Load(SceneChangeRequest request) {
            if (LoadedScenes.Contains(request.SceneConfig)) return null;

            if (request.UseLoadingScreen) {
                _bus.Fire(new LoadingScreenRequestSignal(true));
            }

            if (request.UnloadOther) {
                for (int i = LoadedScenes.Count - 1; i >= 0; i--) {
                    Unload(LoadedScenes[i]);
                }
            }

            LoadedScenes.Add(request.SceneConfig);

            var operation = SceneManager.LoadSceneAsync(request.SceneConfig.sceneName, LoadSceneMode.Additive);
            operation.completed += loadOperation => {
                if (request.SetActive) {
                    var scene = SceneManager.GetSceneByName(request.SceneConfig.sceneName);
                    SceneManager.SetActiveScene(scene);
                }

                if (request.UseLoadingScreen) {
                    _bus.Fire(new LoadingScreenRequestSignal(false));
                }
            };
            return operation;
        }

        public void Unload(SceneConfig sceneConfig) {
            if (!LoadedScenes.Contains(sceneConfig)) return;

            LoadedScenes.Remove(sceneConfig);
            SceneManager.UnloadSceneAsync(sceneConfig.sceneName);
        }
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