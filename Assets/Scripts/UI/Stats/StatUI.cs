using TMPro;
using UnityEngine;

namespace Unbowed.UI.Stats {
    public class StatUI : MonoBehaviour {
        [SerializeField] TMP_Text text;
        [SerializeField] string format;
        [SerializeField] bool floorToInt = true;

        public void SetStat(float value) {
            float displayedValue = floorToInt ? Mathf.FloorToInt(value) : value;
            text.text = string.IsNullOrEmpty(format) ? displayedValue.ToString() : string.Format(format, displayedValue);
        }
    }
}