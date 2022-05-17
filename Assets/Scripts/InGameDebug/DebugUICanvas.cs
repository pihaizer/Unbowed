using Unbowed.Utility;
using UnityEngine;

namespace Unbowed.InGameDebug {
    public class DebugUICanvas : MonoBehaviour {
        private static DebugUICanvas Instance { get; set; }

        private DebugUIController _debugUIController;

        private void Start() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            if (Instance == this) {
                DontDestroyOnLoad(gameObject);
                return;
            }

            Instance = this;

            _debugUIController = GetComponentInChildren<DebugUIController>(true);
            _debugUIController.Init();
            _debugUIController.gameObject.SetActive(false);

            DontDestroyOnLoad(gameObject);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.F12)) _debugUIController.gameObject.ToggleActive();
        }

        public void SetTimescale(float value)
        {
            Time.timeScale = value <= 1 ? value : Mathf.Lerp(1, 10, value - 1);
        }
    }
}