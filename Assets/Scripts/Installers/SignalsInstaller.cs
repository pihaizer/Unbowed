using Unbowed.Gameplay.Signals;
using Unbowed.Signals;
using Zenject;

namespace Unbowed.Installers
{
    public class SignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<LoadingScreenRequestSignal>();
            Container.DeclareSignal<DroppedItemClickedSignal>();
            Container.DeclareSignal<ShowInventoryRequest>();
            Container.DeclareSignal<DescriptionCreateRequestSignal>();
            Container.DeclareSignal<DescriptionShowRequestSignal>();
        }
    }
}