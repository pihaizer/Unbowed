using System;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.Managers {
    public class OnStartLoader : MonoBehaviour {
        [SerializeField] SceneConfig sceneLoaded;

        void Awake() {
            ScenesConfig.Instance.Init();
            if (ScenesConfig.Instance.loadedScenes.Count == 0) {
                ScenesConfig.Instance.Load(new SceneChangeRequest(sceneLoaded) {
                    setActive = true, useLoadingScreen = true
                });
            }
            Destroy(gameObject);
        }
    }
}