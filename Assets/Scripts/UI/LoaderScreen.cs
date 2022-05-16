using System;
using Unbowed.Signals;
using Unbowed.SO;
using UnityEngine;
using Zenject;

namespace Unbowed.UI
{
    public class LoaderScreen : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreenContent;

        [Inject] private SignalBus _bus;

        private int _showLoaderCalls;

        private void Awake()
        {
            _bus.Subscribe<LoadingScreenRequestSignal>(ShowLoadingScreen);
        }

        private void Start()
        {
            loadingScreenContent.gameObject.SetActive(ScenesConfig.Instance.LoadedScenes.Count == 0);
        }

        private void OnDestroy()
        {
            _bus.Unsubscribe<LoadingScreenRequestSignal>(ShowLoadingScreen);
        }

        private void ShowLoadingScreen(LoadingScreenRequestSignal signal)
        {
            _showLoaderCalls += signal.IsOpen ? 1 : -1;
            loadingScreenContent.SetActive(_showLoaderCalls > 0);
        }
    }
}