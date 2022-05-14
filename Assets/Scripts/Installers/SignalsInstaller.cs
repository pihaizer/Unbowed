using Unbowed.Gameplay.Signals;
using Zenject;

namespace Unbowed.Installers
{
    public class SignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<DroppedItemClickedSignal>();
        }
    }
}