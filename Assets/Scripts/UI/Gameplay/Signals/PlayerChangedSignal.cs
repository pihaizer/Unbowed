using Unbowed.Gameplay.Characters;

namespace Unbowed.UI.Gameplay.Signals
{
    public class PlayerChangedSignal
    {
        public Character Player;

        public PlayerChangedSignal(Character player)
        {
            Player = player;
        }
    }
}