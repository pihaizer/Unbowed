using Unbowed.Gameplay;
using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Characters.Modules;

namespace Unbowed.UI.Gameplay.Signals
{
    public class HealthChangedSignal
    {
        public readonly Health Target;
        public readonly Character Source;

        public HealthChangedSignal(Health target, Character source)
        {
            Target = target;
            Source = source;
        }
    }
}