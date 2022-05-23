using Unbowed.Gameplay.Characters;

namespace Unbowed.Gameplay.Signals
{
    public class CharacterRevivedSignal
    {
        public Character Character { get; }
        
        public CharacterRevivedSignal(Character character)
        {
            Character = character;
        }
    }
}