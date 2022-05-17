using System;
using Unbowed.SO;
using UnityEngine;
using Zenject;

namespace Unbowed.Managers {
    public class OnStartLoader : MonoBehaviour {
        [SerializeField] private SceneConfig sceneLoaded;
        [SerializeField] private bool initScenesConfig;
        [SerializeField] private bool setActive;
        [SerializeField] private bool useLoadingScreen;
        [SerializeField] private bool ignoreIfHasScenes;

        [Inject] private IScenesController _scenesController;

        private void Start() {
            if (!_scenesController.LoadedScenes.Contains(sceneLoaded) &&
                (!ignoreIfHasScenes || _scenesController.LoadedScenes.Count == 0)) {
                _scenesController.Load(new SceneChangeRequest(sceneLoaded) {
                    SetActive = setActive, UseLoadingScreen = useLoadingScreen
                });
            }

            Destroy(gameObject);
        }
    }
}