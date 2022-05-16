using System;

using Unbowed.Gameplay;
using Unbowed.Managers;
using Unbowed.SO;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unbowed.UI.Gameplay.WholeScreens {
    public class PauseScreenUI : MonoBehaviour {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button toMainMenuButton;

        [SerializeField] private SceneConfig mainMenu;
        
        [Inject] private IScenesController _scenesController;

        private void Awake() {
            resumeButton.onClick.AddListener(Resume);
            toMainMenuButton.onClick.AddListener(ToMainMenu);
        }

        private void OnEnable() {
            Time.timeScale = 0;
        }

        private void OnDisable() {
            Time.timeScale = 1;
        }

        private void Resume() {
            gameObject.SetActive(false);
        }

        private void ToMainMenu() {
            FindObjectOfType<GameController>().Save();
            _scenesController.Load(new SceneChangeRequest(mainMenu) {
                UnloadOther = true, SetActive = true, UseLoadingScreen = true
            });
        }
    }
}