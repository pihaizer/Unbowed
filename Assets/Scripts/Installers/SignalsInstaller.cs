using Unbowed.Gameplay.Signals;
using Unbowed.Signals;
using Unbowed.UI.Signals;
using Zenject;

namespace Unbowed.Installers
{
    public class SignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<LoadingScreenRequestSignal>();
            Container.DeclareSignal<ScreenActionSignal>();
            Container.DeclareSignal<DroppedItemClickedSignal>();
            Container.DeclareSignal<ItemUiClickedSignal>();
            Container.DeclareSignal<ShowInventoryRequestSignal>();
            Container.DeclareSignal<DescriptionCreateRequestSignal>();
            Container.DeclareSignal<DescriptionShowRequestSignal>();
        }
    }
}