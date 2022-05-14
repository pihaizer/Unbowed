using Unbowed.Managers.Saves;
using Zenject;

namespace Unbowed.Installers
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SaveController>().AsSingle().NonLazy();
        }
    }
}