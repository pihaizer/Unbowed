using HyperCore.UI;
using Unbowed.Gameplay;
using Unbowed.Gameplay.Signals;
using Unbowed.Managers;
using Unbowed.SO;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unbowed.UI.Gameplay.Popups
{
    public class DeathScreenUI : CanvasScreen
    {
        [SerializeField] private Button reviveButton;
        [SerializeField] private Button toMainMenuButton;
        [SerializeField] private SceneConfig mainMenuScene;

        [Inject] private IScenesController _scenesController;
        [Inject] private SignalBus _bus;

        protected override void Awake()
        {
            base.Awake();
            _bus.Subscribe<CharacterDiedSignal>(OnCharacterDied);
            _bus.Subscribe<CharacterRevivedSignal>(OnCharacterRevived);
            reviveButton.onClick.AddListener(Revive);
            toMainMenuButton.onClick.AddListener(ToMainMenu);
        }

        private void OnDestroy()
        {
            _bus.Unsubscribe<CharacterDiedSignal>(OnCharacterDied);
            _bus.Unsubscribe<CharacterRevivedSignal>(OnCharacterRevived);
        }

        private void OnCharacterDied(CharacterDiedSignal signal)
        {
            if (signal.Character.Type is not CharacterType.Player) return;
            Open();
        }

        private void OnCharacterRevived(CharacterRevivedSignal signal)
        {
            if (signal.Character.Type is not CharacterType.Player) return;
            Close();
        }

        private void Revive()
        {
            ActivePlayer.Revive();
        }

        private void ToMainMenu()
        {
            FindObjectOfType<GameController>().Save();
            _scenesController.Load(new SceneChangeRequest(mainMenuScene)
                { SetActive = true, UnloadOther = true, UseLoadingScreen = true });
        }
    }
}