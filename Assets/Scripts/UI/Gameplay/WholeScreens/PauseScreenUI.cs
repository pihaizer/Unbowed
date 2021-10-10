using System;

using Unbowed.Gameplay;
using Unbowed.SO;
using UnityEngine;
using UnityEngine.UI;

namespace Unbowed.UI.Gameplay.WholeScreens {
    public class PauseScreenUI : MonoBehaviour {
        [SerializeField] Button resumeButton;
        [SerializeField] Button optionsButton;
        [SerializeField] Button toMainMenuButton;

        [SerializeField] SceneConfig mainMenu;

        void Awake() {
            resumeButton.onClick.AddListener(Resume);
            toMainMenuButton.onClick.AddListener(ToMainMenu);
        }

        void OnEnable() {
            Time.timeScale = 0;
        }

        void OnDisable() {
            Time.timeScale = 1;
        }

        void Resume() {
            gameObject.SetActive(false);
        }

        void ToMainMenu() {
            FindObjectOfType<GameController>().Save();
            ScenesConfig.Instance.Load(new SceneChangeRequest(mainMenu) {
                unloadOther = true, setActive = true, useLoadingScreen = true
            });
        }
    }
}