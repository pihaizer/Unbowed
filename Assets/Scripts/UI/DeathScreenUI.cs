using System;
using SO.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class DeathScreenUI : MonoBehaviour {
        [SerializeField] Button reviveButton;
        [SerializeField] Button toMainMenuButton;

        [SerializeField] SceneLoadRequestEventSO sceneLoadRequestEvent;

        void Awake() {
            reviveButton.onClick.AddListener(Revive);
            toMainMenuButton.onClick.AddListener(ToMainMenu);
            gameObject.SetActive(false);
        }

        void Revive() {
            gameObject.SetActive(false);
            // sceneLoadRequestEvent.Invoke(new SceneLoadRequestEventSO.SceneLoadRequestData() {
            //     hasLoadScreen = true,
            //     isLoad = true,
            //     name = SceneManager.GetActiveScene().name
            // });
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