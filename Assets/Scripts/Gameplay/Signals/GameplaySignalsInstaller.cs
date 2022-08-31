using Unbowed.UI.Gameplay.Signals;
using Zenject;

namespace Unbowed.Gameplay.Signals
{
    public class GameplaySignalsInstaller:MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.DeclareSignalWithInterfaces<CharacterDiedSignal>();
            Container.DeclareSignal<CharacterRevivedSignal>();
            Container.DeclareSignal<CharacterLeveledUp>();
            Container.DeclareSignal<PlayerChangedSignal>();
        }
    }
}