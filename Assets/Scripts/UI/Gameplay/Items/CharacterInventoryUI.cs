using HyperCore.UI;
using Sirenix.OdinInspector;
using Unbowed.UI.Signals;
using UnityEngine;
using Zenject;

namespace Unbowed.UI.Gameplay.Items
{
    public class CharacterInventoryUI : CanvasScreen
    {
        [SerializeField, Required, ChildGameObjectsOnly]
        private BagsUI bagsUI;

        [SerializeField, Required, ChildGameObjectsOnly]
        private EquipmentUI equipmentUI;

        [SerializeField] private bool autoSubscribeToSignal = true;

        [Inject] private SignalBus _bus;

        private Unbowed.Gameplay.Characters.Modules.Inventory Inventory { get; set; }

        protected override void Awake()
        {
            base.Awake();
            if (autoSubscribeToSignal) this.SubscribeToAction(_bus, ScreenNames.PlayerInventory);
        }

        public void SetInventory(Unbowed.Gameplay.Characters.Modules.Inventory inventory)
        {
            Inventory = inventory;
            if (Inventory == null) return;
            equipmentUI.SetInventory(Inventory);
            bagsUI.SetInventory(Inventory);
        }
    }
}