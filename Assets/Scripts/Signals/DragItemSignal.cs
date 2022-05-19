using Unbowed.UI.Gameplay.Items;
using UnityEngine.EventSystems;

namespace Unbowed.Signals
{
    public class DragItemSignal
    {
        public ItemUI ItemUI { get; }
        public PointerEventData EventData { get; }

        public DragItemSignal(ItemUI itemUI, PointerEventData eventData)
        {
            ItemUI = itemUI;
            EventData = eventData;
        }
    }
}