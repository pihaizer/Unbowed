using System;
using Sirenix.OdinInspector;
using Unbowed.SO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Unbowed.UI.MainMenu {
    public class MainMenu : MonoBehaviour {
        [SerializeField, ChildGameObjectsOnly] private Button newGameButton;

        [SerializeField, ChildGameObjectsOnly] private Button optionsButton;

        [SerializeField, ChildGameObjectsOnly] private Button exitButton;

        [SerializeField] private SceneConfig gameSceneConfig;

        private void Awake() {
            newGameButton.onClick.AddListener(StartNewGame);
            optionsButton.onClick.AddListener(OpenOptions);
            exitButton.onClick.AddListener(Exit);
        }

        private void StartNewGame() {
            ScenesConfig.Instance.Load(new SceneChangeRequest(gameSceneConfig) {
                useLoadingScreen = true, setActive = true, unloadOther = true
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