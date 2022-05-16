using Unbowed.Gameplay.Characters.Configs.Stats.Configs;
using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.SO;
using UnityEngine;
using Zenject;

namespace Unbowed.Installers
{
    public class ConfigsInstaller : MonoInstaller
    {
        [SerializeField] private AllStatModifiers allStatModifiersConfig;
        [SerializeField] private AllItemsConfig allItemsConfig;
        [SerializeField] private ScenesConfig scenesConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<AllStatModifiers>().FromInstance(allStatModifiersConfig);
            Container.Bind<AllItemsConfig>().FromInstance(allItemsConfig);
            Container.Bind<ScenesConfig>().FromInstance(scenesConfig);
            

            foreach (ItemConfig itemConfig in allItemsConfig.allItems)
            {
                Container.QueueForInject(itemConfig);
            }
        }
    }
}