using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.Gameplay.Items;
using Unbowed.SO;
using UnityEngine;
using Zenject;

namespace Unbowed.Gameplay.Characters.Items
{
    public class ItemsDropper : MonoInstaller
    {
        [SerializeField, AssetsOnly] private DroppedItem _droppedItemPrefab;

        public override void InstallBindings()
        {
            Container.Bind<ItemsDropper>().FromInstance(this);
        }

        public void DropItem(Item item, Transform transform)
        {
            Inventory.RemoveItem(item);
            DroppedItem droppedItem = Instantiate(_droppedItemPrefab);
            droppedItem.transform.position = transform.position + Vector3.up;
            droppedItem.SetItem(item);
        }
    }
}