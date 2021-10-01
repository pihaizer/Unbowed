using Unbowed.Gameplay.Characters;
using Unbowed.SO.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Unbowed.UI {
    public class DeathScreenUI : MonoBehaviour {
        [SerializeField] Button reviveButton;
        [SerializeField] Button toMainMenuButton;

        [SerializeField] SceneLoadRequestEventSO sceneLoadRequestEvent;

        void Awake() {
            reviveButton.onClick.AddListener(Revive);
            toMainMenuButton.onClick.AddListener(ToMainMenu);
        }

        void Revive() {
            gameObject.SetActive(false);
            FindObjectOfType<PlayerCharacter>().health.Revive();
        }

        void ToMainMenu() {
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}