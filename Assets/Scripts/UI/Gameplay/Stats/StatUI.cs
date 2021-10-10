using TMPro;

using Unbowed.Gameplay.Characters.Configs.Stats;

using UnityEngine;

namespace Unbowed.UI.Gameplay.Stats {
    public class StatUI : MonoBehaviour {
        public StatType type;
        
        [SerializeField] TMP_Text text;
        [SerializeField] string format;
        [SerializeField] bool floorToInt = true;

        public void Init(Unbowed.Gameplay.Characters.Configs.Stats.Stats stats) {
            float value = stats[type];
            float displayedValue = floorToInt ? Mathf.FloorToInt(value) : value;
            text.text = string.IsNullOrEmpty(format) ? displayedValue.ToString() : string.Format(format, displayedValue);
        }
    }
}