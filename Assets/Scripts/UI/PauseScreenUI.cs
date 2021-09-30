using UnityEngine;
using UnityEngine.UI;

namespace Unbowed.UI {
    public class PauseScreenUI : MonoBehaviour {
        [SerializeField] Button resumeButton;
        [SerializeField] Button saveAndExitButton;
        [SerializeField] Button optionsButton;

        void Start() {
            resumeButton.onClick.AddListener(Resume);
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
    }
}