using Unbowed.Gameplay;
using Unbowed.Managers;
using Unbowed.SO;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unbowed.UI.Gameplay.WholeScreens {
    public class DeathScreenUI : MonoBehaviour {
        [SerializeField] private Button reviveButton;
        [SerializeField] private Button toMainMenuButton;
        [SerializeField] private SceneConfig mainMenuScene;

        [Inject] private IScenesController _scenesController;

        private void Awake() {
            ActivePlayer.Died += OnPlayerDied;
            ActivePlayer.Revived += OnPlayerRevived;
            reviveButton.onClick.AddListener(Revive);
            toMainMenuButton.onClick.AddListener(ToMainMenu);
        }

        private void OnDestroy() {
            ActivePlayer.Died -= OnPlayerDied;
            ActivePlayer.Revived -= OnPlayerRevived;
        }

        private void OnPlayerDied(DeathData data) => gameObject.SetActive(true);

        private void OnPlayerRevived() => gameObject.SetActive(false);

        private void Revive() => ActivePlayer.Revive();

        private void ToMainMenu() {
            FindObjectOfType<GameController>().Save();
            _scenesController.Load(new SceneChangeRequest(mainMenuScene)
                {SetActive = true, UnloadOther = true, UseLoadingScreen = true});
        }
    }
}