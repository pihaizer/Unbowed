using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.SO;
using Unbowed.UI.Stats;
using UnityEngine;

namespace Unbowed.UI.Gameplay.Stats {
    public class StatsUI : Menu {
        [OdinSerialize]
        Dictionary<StatType, StatUI> _statUis = Enum.GetValues(typeof(StatType)).Cast<StatType>()
            .ToDictionary((type) => type, (type) => (StatUI) null);

        protected override void Awake() {
            base.Awake();
            ActivePlayer.StatsUpdated += UpdateStats;
            ActivePlayer.PlayerChanged += UpdateStats;
            UpdateStats();
        }

        void UpdateStats() {
            var stats = ActivePlayer.GetStats();
            if (stats?.Values == null) return;
            foreach (var stat in stats.Values) {
                _statUis[stat.Key].SetStat(stat.Value);
            }
        }


        [ContextMenu("Refresh dict")]
        void RefreshStatsDict() {
            _statUis = Enum.GetValues(typeof(StatType)).Cast<StatType>()
                .ToDictionary((type) => type, (type) => (StatUI) null);
        }
    }
}