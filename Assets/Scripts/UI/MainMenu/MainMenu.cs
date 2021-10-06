using System;
using Sirenix.OdinInspector;
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

        void Awake() {
            newGameButton.onClick.AddListener(StartNewGame);
            optionsButton.onClick.AddListener(OpenOptions);
            exitButton.onClick.AddListener(Exit);
        }

        void StartNewGame() {
            throw new NotImplementedException();
        }

        void OpenOptions() { }

        void Exit() {
            if (Application.isEditor) {
                EditorApplication.isPlaying = false;
            } else {
                Application.Quit();
            }
        }
    }
}