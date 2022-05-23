using HyperCore.UI;
using Unbowed.Gameplay;
using Unbowed.Managers;
using Unbowed.SO;
using Unbowed.UI.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unbowed.UI.Gameplay.Popups
{
    public class PauseScreenUI : CanvasScreen
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button toMainMenuButton;

        [SerializeField] private SceneConfig mainMenu;

        [Inject] private IScenesController _scenesController;
        [Inject] private SignalBus _signalBus;

        protected override void Awake()
        {
            base.Awake();
            resumeButton.onClick.AddListener(Resume);
            toMainMenuButton.onClick.AddListener(ToMainMenu);
            this.SubscribeToAction(_signalBus, ScreenNames.Pause);
        }

        protected override void SetOpenedInternal(bool value)
        {
            base.SetOpenedInternal(value);
            Time.timeScale = value ? 0 : 1;
        }

        private void Resume() => Close();

        private void ToMainMenu()
        {
            FindObjectOfType<GameController>().Save();
            _scenesController.Load(new SceneChangeRequest(mainMenu)
            {
                UnloadOther = true, SetActive = true, UseLoadingScreen = true
            });
        }
    }
}