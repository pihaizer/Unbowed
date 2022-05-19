using HyperCore.UI;

namespace Unbowed.UI
{
    public class GameObjectScreen : BaseScreen
    {
        public override BaseScreen Parent { get; protected set; }
        
        protected override void SetOpenedInternal(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}