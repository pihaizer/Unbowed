using HyperCore.UI;
using Unbowed.UI.Gameplay.Inventory;
using Unbowed.UI.Signals;
using UnityEngine;
using Zenject;

namespace Unbowed.UI.Gameplay
{
    public class OtherInventoryUI : CanvasScreen
    {
        [SerializeField] private BagsUI bagsUI;
        [Inject] private SignalBus _bus;
        
        
        protected override void Awake()
        {
            base.Awake();
            this.SubscribeToAction(_bus, ScreenNames.OtherInventory, DataAction);
        }

        private void DataAction(object data)
        {
            bagsUI.SetInventory(data as Unbowed.Gameplay.Characters.Modules.Inventory);
        }
    }
}