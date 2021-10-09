using System;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.Managers {
    public class OnStartLoader : MonoBehaviour {
        [SerializeField] SceneConfig sceneLoaded;
        [SerializeField] bool initScenesConfig;
        [SerializeField] bool setActive;
        [SerializeField] bool useLoadingScreen;
        [SerializeField] bool ignoreIfHasScenes;

        void Awake() {
            if (initScenesConfig) ScenesConfig.Instance.Init();
            
            if (!ScenesConfig.Instance.loadedScenes.Contains(sceneLoaded) &&
                (!ignoreIfHasScenes || ScenesConfig.Instance.loadedScenes.Count == 0)) {
                ScenesConfig.Instance.Load(new SceneChangeRequest(sceneLoaded) {
                    setActive = setActive, useLoadingScreen = useLoadingScreen
                });
            }

            Destroy(gameObject);
        }
    }
}