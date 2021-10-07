using Unbowed.Utility;
using UnityEngine;

namespace Unbowed.InGameDebug {
    public class DebugUICanvas : MonoBehaviour {
        static DebugUICanvas Instance { get; set; }

        DebugUIController _debugUIController;

        void Start() {
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

        void Update() {
            if (Input.GetKeyDown(KeyCode.F12)) _debugUIController.gameObject.ToggleActive();
        }
    }
}