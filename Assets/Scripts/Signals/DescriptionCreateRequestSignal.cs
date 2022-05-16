using Unbowed.Gameplay.Items;

namespace Unbowed.Signals
{
    public class DescriptionCreateRequestSignal
    {
        public DroppedItem Item;
        public bool IsCreate;

        public DescriptionCreateRequestSignal(DroppedItem item, bool isCreate)
        {
            Item = item;
            IsCreate = isCreate;
        }
    }
}