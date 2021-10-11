using Unbowed.Gameplay.Characters;

using UnityEngine;

namespace Unbowed.Gameplay {
    public interface IHittable {
        void Hit(int damage, Character source);

        bool CanBeHit();

        GameObject GetGameObject();
    }
}