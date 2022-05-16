using Unbowed.Managers;
using Unbowed.Managers.Saves;
using Zenject;

namespace Unbowed.Installers
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISaveController>().To<SaveController>().AsSingle().NonLazy();
            
            Container.BindInterfacesTo<ScenesController>().AsSingle().NonLazy();
            // Container.Bind<IScenesController>().To<ScenesController>().AsSingle().NonLazy();
        }
    }
}