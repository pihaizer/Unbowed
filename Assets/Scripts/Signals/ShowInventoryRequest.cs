using Unbowed.Gameplay.Characters.Modules;

namespace Unbowed.Signals
{
    public class ShowInventoryRequest
    {
        public Inventory Inventory;
        public bool IsOpen;

        public ShowInventoryRequest(Inventory inventory, bool isOpen)
        {
            Inventory = inventory;
            IsOpen = isOpen;
        }
    }
}