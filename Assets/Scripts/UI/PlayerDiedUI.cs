using System;
using SO.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class PlayerDiedUI : MonoBehaviour {
        [SerializeField] Button restartButton;
        [SerializeField] Button exitButton;

        [SerializeField] SceneLoadRequestEventSO sceneLoadRequestEvent;

        void Awake() {
            restartButton.onClick.AddListener(Restart);
            exitButton.onClick.AddListener(Exit);
            gameObject.SetActive(false);
        }

        void Restart() {
            gameObject.SetActive(false);
            sceneLoadRequestEvent.Invoke(new SceneLoadRequestEventSO.SceneLoadRequestData() {
                hasLoadScreen = true,
                isLoad = true,
                name = SceneManager.GetActiveScene().name
            });
        }

        void Exit() {
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