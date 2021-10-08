using System;
using Sirenix.OdinInspector;
using Unbowed.SO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Unbowed.UI.MainMenu {
    public class MainMenu : MonoBehaviour {
        [SerializeField, ChildGameObjectsOnly]
        Button newGameButton;

        [SerializeField, ChildGameObjectsOnly]
        Button optionsButton;

        [SerializeField, ChildGameObjectsOnly]
        Button exitButton;

        [SerializeField]
        SceneConfig gameSceneConfig;

        void Awake() {
            newGameButton.onClick.AddListener(StartNewGame);
            optionsButton.onClick.AddListener(OpenOptions);
            exitButton.onClick.AddListener(Exit);
        }

        void StartNewGame() {
            ScenesConfig.Instance.Load(new SceneChangeRequest(gameSceneConfig) {
                useLoadingScreen = true, setActive = true, unloadOther = true
            });
        }

        void OpenOptions() { }

        void Exit() {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}