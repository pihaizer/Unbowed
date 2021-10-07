using System;
using Unbowed.Gameplay;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.UI.Gameplay {
    public class PlayerHealthBarUI : HealthBarUI {
        void Start() {
            ActivePlayer.HealthChanged += OnHealthChanged;
            var health = ActivePlayer.GetHealth();
            if (health != null) SetHealthPercent((float) health.Current / health.Max);
        }

        void OnDestroy() {
            ActivePlayer.HealthChanged -= OnHealthChanged;
        }
    }
}