using System.ComponentModel;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.AI.Brains;
using Unbowed.Managers;
using Unbowed.Managers.Saves;
using Unbowed.SO.Brains;
using Unbowed.UI.Gameplay;
using UnityEngine;
using Zenject;

namespace Unbowed.Installers
{
    public class ServicesInstaller : MonoInstaller
    {
        [SerializeField, AssetsOnly] private ItemDragger itemDraggerPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<ISaveController>().To<SaveController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<ScenesController>().AsSingle().NonLazy();
            
            Container.BindFactory<BrainConfigSO, Brain, BrainFactory>()
                .FromFactory<InjectingBrainFactory>();
            Container.Bind<ItemDragger>().FromComponentInNewPrefab(itemDraggerPrefab).AsSingle();
        }
    }
}