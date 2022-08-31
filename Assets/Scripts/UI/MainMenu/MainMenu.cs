using System;
using Sirenix.OdinInspector;
using Unbowed.Managers;
using Unbowed.SO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unbowed.UI.MainMenu {
    public class MainMenu : MonoBehaviour {
        [SerializeField, ChildGameObjectsOnly] private Button newGameButton;
        
        // [SerializeField, ChildGameObjectsOnly] private Button continueButton;

        [SerializeField, ChildGameObjectsOnly] private Button optionsButton;

        [SerializeField, ChildGameObjectsOnly] private Button exitButton;

        [SerializeField] private SceneConfig gameSceneConfig;

        [Inject] private IScenesController _scenesController;

        private void Awake() {
            newGameButton.onClick.AddListener(StartNewGame);
            // continueButton.onClick.AddListener(StartNewGame);
            optionsButton.onClick.AddListener(OpenOptions);
            exitButton.onClick.AddListener(Exit);
        }

        public void StartNewGame() {
            _scenesController.Load(new SceneChangeRequest(gameSceneConfig) {
                UseLoadingScreen = true, SetActive = true, UnloadOther = true
            });
        }

        private void OpenOptions() { }

        private void Exit() {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}