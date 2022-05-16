using Unbowed.Gameplay.Characters.Configs.Stats.Configs;
using Unbowed.Gameplay.Characters.Items.Configs;
using UnityEngine;
using Zenject;

namespace Unbowed.Configs
{
    public class ConfigsInstaller : MonoInstaller
    {
        [SerializeField] private AllStatModifiers allStatModifiersConfig;
        [SerializeField] private AllItemsConfig allItemsConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<AllStatModifiers>().FromInstance(allStatModifiersConfig);
            Container.Bind<AllItemsConfig>().FromInstance(allItemsConfig);
            

            foreach (ItemConfig itemConfig in allItemsConfig.allItems)
            {
                Container.QueueForInject(itemConfig);
            }
        }
    }
}