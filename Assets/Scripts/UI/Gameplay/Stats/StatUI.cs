using TMPro;

using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Stats;
using Unbowed.Gameplay.Characters.Stats.Configs;

using UnityEngine;

namespace Unbowed.UI.Gameplay.Stats {
    public class StatUI : MonoBehaviour {
        public StatType type;
        
        [SerializeField] private TMP_Text text;
        [SerializeField] private string format;
        [SerializeField] private bool floorToInt = true;

        public void Init(Unbowed.Gameplay.Characters.Stats.Stats stats) {
            float value = stats[type];
            float displayedValue = floorToInt ? Mathf.FloorToInt(value) : value;
            text.text = string.IsNullOrEmpty(format) ? displayedValue.ToString() : string.Format(format, displayedValue);
        }
    }
}