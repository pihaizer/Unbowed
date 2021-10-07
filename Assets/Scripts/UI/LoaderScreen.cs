using System;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.UI {
    public class LoaderScreen : MonoBehaviour {
        [SerializeField] GameObject loadingScreenContent;

        int _showLoaderCalls = 0;

        void Start() {
            loadingScreenContent.gameObject.SetActive(SceneDirector.Instance.loadedScenes.Count == 0);
            EventsContext.Instance.showLoadingScreen += ShowLoadingScreen;
        }

        void ShowLoadingScreen(bool value) {
            _showLoaderCalls += value ? 1 : -1;
            loadingScreenContent.SetActive(_showLoaderCalls > 0);
        }
    }
}