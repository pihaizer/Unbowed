using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.UI.Stats {
    public class StatsUI : Menu {
        [OdinSerialize]
        Dictionary<StatType, StatUI> _statUis =
            Enum.GetValues(typeof(StatType)).Cast<StatType>()
                .ToDictionary((type) => type, (type) => (StatUI) null);

        protected override void Awake() {
            base.Awake();
        }

        void OnEnable() {
            var player = GlobalContext.Instance.playerCharacter;
            if (player) {
                player.stats.Updated += UpdateStats;
                UpdateStats();
            }
        }

        void OnDisable() {
            var player = GlobalContext.Instance.playerCharacter;
            if (player) {
                player.stats.Updated -= UpdateStats;
            }
        }

        void UpdateStats() {
            var player = GlobalContext.Instance.playerCharacter;
            if (!player) return;
            foreach (var stat in player.stats.Values) {
                _statUis[stat.Key].SetStat(stat.Value);
            }
        }


        [ContextMenu("Refresh dict")]
        void RefreshStatsDict() {
            _statUis =
                Enum.GetValues(typeof(StatType)).Cast<StatType>()
                    .ToDictionary((type) => type, (type) => (StatUI) null);
        }
    }
}