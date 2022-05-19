using Unbowed.UI.Gameplay.Items;
using UnityEngine;
using Zenject;

namespace Unbowed.Installers
{
    public class GameplayUiInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<Object, ItemUI, ItemUI.Factory>().FromFactory<PrefabFactory<ItemUI>>();
        }
    }
}