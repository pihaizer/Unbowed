using System;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.UI {
    public class LoaderScreen : MonoBehaviour {
        [SerializeField] private GameObject loadingScreenContent;

        private int _showLoaderCalls = 0;

        private void Start() {
            loadingScreenContent.gameObject.SetActive(ScenesConfig.Instance.loadedScenes.Count == 0);
            EventsContext.Instance.showLoadingScreen += ShowLoadingScreen;
        }

        private void ShowLoadingScreen(bool value) {
            _showLoaderCalls += value ? 1 : -1;
            loadingScreenContent.SetActive(_showLoaderCalls > 0);
        }
    }
}