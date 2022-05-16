using Unbowed.Gameplay.Items;

namespace Unbowed.Signals
{
    public class DescriptionShowRequestSignal
    {
        public DroppedItem Item;
        public bool IsShow;

        public DescriptionShowRequestSignal(DroppedItem item, bool isShow)
        {
            Item = item;
            IsShow = isShow;
        }
    }
}