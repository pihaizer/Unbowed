using TMPro;
using UnityEngine;

namespace UI.Debug {
    public class ErrorViewUIController : MonoBehaviour {
        public TMP_Text errorMessageText;
        public TMP_Text stackTraceText;

        public void Open(string text, string stackTrace) {
            gameObject.SetActive(true);
            errorMessageText.text = text;
            stackTraceText.text = stackTrace;
        }
    }
}