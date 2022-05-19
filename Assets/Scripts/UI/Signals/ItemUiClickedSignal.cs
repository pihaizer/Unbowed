using Unbowed.UI.Gameplay.Items;

namespace Unbowed.UI.Signals
{
    public class ItemUiClickedSignal
    {
        public ItemUI ItemUI { get; } 
        
        public ItemUiClickedSignal(ItemUI itemUI)
        {
            ItemUI = itemUI;
        }
    }
}