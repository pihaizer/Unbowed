using System;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.Managers {
    public class OnStartLoader : MonoBehaviour {
        [SerializeField] SceneConfig sceneLoaded;

        void Awake() {
            SceneDirector.Instance.Init();
            if (SceneDirector.Instance.loadedScenes.Count == 0) {
                SceneDirector.Instance.Load(new SceneChangeRequest(sceneLoaded) {
                    setActive = true, useLoadingScreen = true
                });
            }
            Destroy(gameObject);
        }
    }
}