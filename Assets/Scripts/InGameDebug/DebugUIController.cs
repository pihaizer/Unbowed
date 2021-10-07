using System;
using TMPro;
using UI.Debug;
using Unbowed.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Unbowed.InGameDebug {
    public class DebugUIController : MonoBehaviour {
        [Header("Buttons")]
        public Button closeButton;
        public Button switchGraphyButton;
        public Button testErrorButton;

        [Header("Sliders")]
        public Slider timeScaleSlider;

        [Header("Other")]
        public ErrorViewUIController errorView;

        public GameObject graphyPrefab;
        public VerticalLayoutGroup consoleLayoutGroup;
        public TMP_Text consoleTextExampleMessage;
        GameObject _graphy;

        public void Init() {
            closeButton.onClick.AddListener(() => gameObject.SetActive(false));
            testErrorButton.onClick.AddListener(() => throw new Exception("Test"));

            _graphy = Instantiate(graphyPrefab);
            DontDestroyOnLoad(_graphy);
            _graphy.SetActive(false);
            switchGraphyButton.onClick.AddListener(_graphy.ToggleActive);

            timeScaleSlider.onValueChanged.AddListener((value) => Time.timeScale = value);

            errorView.gameObject.SetActive(false);

            Application.logMessageReceived += LogToConsole;
            consoleTextExampleMessage.gameObject.SetActive(false);
        }

        void LogToConsole(string logString, string stackTrace, LogType type) {
            if (consoleLayoutGroup == null) return;
            var newMessage = Instantiate(consoleTextExampleMessage, consoleLayoutGroup.transform, false);
            newMessage.gameObject.SetActive(true);
            newMessage.text = logString;
            switch (type) {
                case LogType.Error:
                case LogType.Exception:
                    newMessage.color = Color.red;
                    break;
                case LogType.Warning:
                case LogType.Assert:
                    newMessage.color = Color.yellow;
                    break;
                case LogType.Log:
                default:
                    newMessage.color = Color.black;
                    break;
            }

            newMessage.GetComponent<Button>().onClick.AddListener(() => errorView.Open(logString, stackTrace));
        }
    }
}