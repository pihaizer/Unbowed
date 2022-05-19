using HyperCore.UI;
using Unbowed.UI.Gameplay.Stats;
using Unbowed.UI.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unbowed.UI.Gameplay
{
    public class BottomBarUI : CanvasScreen
    {
        [SerializeField] private Button characterButton;
        [SerializeField] private Button inventoryButton;

        [Inject] private SignalBus _bus;

        protected override void Awake()
        {
            characterButton.onClick.AddListener(RequestStats);
            inventoryButton.onClick.AddListener(RequestInventory);
        }

        private void RequestInventory() => 
            _bus.Fire(new ScreenActionSignal(ScreenNames.PlayerInventory, ScreenAction.Switch));

        private void RequestStats() => 
            _bus.Fire(new ScreenActionSignal(ScreenNames.Stats, ScreenAction.Switch));
    }
}