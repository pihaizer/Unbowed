using Unbowed.Configs;
using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.SO;
using UnityEngine;
using Zenject;

namespace Unbowed.Installers
{
    public class ConfigsInstaller : MonoInstaller
    {
        [SerializeField] private GlobalConfig globalConfig;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GlobalConfig>().FromInstance(globalConfig);
            Container.Bind<AllItemsConfig>().FromInstance(globalConfig.AllItemsConfig);
            Container.Bind<AllStatModifiers>().FromInstance(globalConfig.AllStatModifiers);
            Container.Bind<ScenesConfig>().FromInstance(globalConfig.ScenesConfig);
        }
    }
}