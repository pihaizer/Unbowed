using Unbowed.Gameplay.Characters;

namespace Unbowed.Gameplay.Signals
{
    public class CharacterDiedSignal : IExperienceGiver
    {
        public Character Character { get; }
        public Character Source { get; }

        public long ExperienceGained => Character.Config.ExperienceConfig.ExperienceOnKill;

        public CharacterDiedSignal(Character character, Character source)
        {
            Character = character;
            Source = source;
        }
    }
}