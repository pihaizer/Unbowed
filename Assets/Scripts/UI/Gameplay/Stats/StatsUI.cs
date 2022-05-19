using System;
using System.Collections.Generic;
using System.Linq;
using HyperCore.UI;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.SO;
using Unbowed.UI.Signals;
using UnityEngine;
using Zenject;

namespace Unbowed.UI.Gameplay.Stats
{
    public class StatsUI : CanvasScreen
    {
        [SerializeField] private List<StatUI> statUis = new List<StatUI>();
        [SerializeField] private bool autoSubscribeToSignal = true;

        [Inject] private SignalBus _bus;

        protected override void Awake()
        {
            base.Awake();
            ActivePlayer.StatsUpdated += UpdateStats;
            ActivePlayer.PlayerChanged += UpdateStats;
            UpdateStats();
            if (autoSubscribeToSignal) this.SubscribeToAction(_bus, ScreenNames.Stats);
        }

        private void UpdateStats()
        {
            var stats = ActivePlayer.GetStats();
            if (stats == null) return;
            foreach (var stat in statUis)
            {
                stat.Init(stats);
            }
        }
    }
}