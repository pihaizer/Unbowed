using HyperCore.UI;
using Unbowed.Gameplay;
using Unbowed.Managers;
using Unbowed.SO;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unbowed.UI.Gameplay.WholeScreens {
    public class DeathScreenUI : CanvasScreen {
        [SerializeField] private Button reviveButton;
        [SerializeField] private Button toMainMenuButton;
        [SerializeField] private SceneConfig mainMenuScene;

        [Inject] private IScenesController _scenesController;

        protected override void Awake() {
            base.Awake();
            ActivePlayer.Died += OnPlayerDied;
            ActivePlayer.Revived += OnPlayerRevived;
            reviveButton.onClick.AddListener(Revive);
            toMainMenuButton.onClick.AddListener(ToMainMenu);
        }

        private void OnDestroy() {
            ActivePlayer.Died -= OnPlayerDied;
            ActivePlayer.Revived -= OnPlayerRevived;
        }

        private void OnPlayerDied(DeathData data) => Open();

        private void OnPlayerRevived() => Close();

        private void Revive() => ActivePlayer.Revive();

        private void ToMainMenu() {
            FindObjectOfType<GameController>().Save();
            _scenesController.Load(new SceneChangeRequest(mainMenuScene)
                {SetActive = true, UnloadOther = true, UseLoadingScreen = true});
        }
    }
}