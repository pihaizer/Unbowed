using System;
using Unbowed.Gameplay.Characters.Player;
using Unbowed.SO;
using UnityEngine;
using UnityEngine.UI;

namespace Unbowed.UI.Gameplay.WholeScreens {
    public class DeathScreenUI : MonoBehaviour {
        [SerializeField] Button reviveButton;
        [SerializeField] Button toMainMenuButton;
        [SerializeField] SceneConfig mainMenuScene;

        void Awake() {
            ActivePlayer.Died += OnPlayerDied;
            ActivePlayer.Revived += OnPlayerRevived;
            reviveButton.onClick.AddListener(Revive);
            toMainMenuButton.onClick.AddListener(ToMainMenu);
        }

        void OnDestroy() {
            ActivePlayer.Died -= OnPlayerDied;
            ActivePlayer.Revived -= OnPlayerRevived;
        }

        void OnPlayerDied() => gameObject.SetActive(true);

        void OnPlayerRevived() => gameObject.SetActive(false);

        void Revive() {
            ActivePlayer.Revive();
        }

        void ToMainMenu() {
            SceneDirector.Instance.Load(new SceneChangeRequest(mainMenuScene)
                {setActive = true, unloadOther = true, useLoadingScreen = true});
        }
    }
}