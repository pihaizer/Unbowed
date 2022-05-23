using Unbowed.Gameplay.Characters;

namespace Unbowed.Gameplay.Signals
{
    public class CharacterLeveledUp
    {
        public Character Character;
        public int NewLevel;

        public CharacterLeveledUp(Character character, int newLevel)
        {
            Character = character;
            NewLevel = newLevel;
        }
    }
}