using UnityEngine;
using Zenject;

namespace Unbowed.UI
{
    public class PopupFactoryInstaller : MonoInstaller
    {
        [SerializeField] private PopupFactory popupFactory;
        
        public override void InstallBindings()
        {
            Container.Bind<PopupFactory>().FromInstance(popupFactory);
        }
    }
}