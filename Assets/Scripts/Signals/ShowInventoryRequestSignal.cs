using Unbowed.Gameplay.Characters.Modules;

namespace Unbowed.Signals
{
    public class ShowInventoryRequestSignal
    {
        public Inventory Inventory;
        public bool IsOpen;

        public ShowInventoryRequestSignal(Inventory inventory, bool isOpen)
        {
            Inventory = inventory;
            IsOpen = isOpen;
        }
    }
}