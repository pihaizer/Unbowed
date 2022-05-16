using System;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.Managers {
    public class OnStartLoader : MonoBehaviour {
        [SerializeField] private SceneConfig sceneLoaded;
        [SerializeField] private bool initScenesConfig;
        [SerializeField] private bool setActive;
        [SerializeField] private bool useLoadingScreen;
        [SerializeField] private bool ignoreIfHasScenes;

        private void Awake() {
            if (initScenesConfig) ScenesConfig.Instance.Init();
            
            if (!ScenesConfig.Instance.LoadedScenes.Contains(sceneLoaded) &&
                (!ignoreIfHasScenes || ScenesConfig.Instance.LoadedScenes.Count == 0)) {
                ScenesConfig.Instance.Load(new SceneChangeRequest(sceneLoaded) {
                    SetActive = setActive, UseLoadingScreen = useLoadingScreen
                });
            }

            Destroy(gameObject);
        }
    }
}