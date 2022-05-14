using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.SO;

using UnityEngine;

namespace Unbowed.UI.Gameplay.Stats {
    public class StatsUI : Menu {
        [SerializeField] private List<StatUI> statUis = new List<StatUI>();

        protected override void Awake() {
            base.Awake();
            ActivePlayer.StatsUpdated += UpdateStats;
            ActivePlayer.PlayerChanged += UpdateStats;
            UpdateStats();
        }

        private void UpdateStats() {
            var stats = ActivePlayer.GetStats();
            if (stats == null) return;
            foreach (var stat in statUis) {
                stat.Init(stats);
            }
        }
    }
}