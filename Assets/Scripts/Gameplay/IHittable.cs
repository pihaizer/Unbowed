using UnityEngine;

namespace Unbowed.Gameplay {
    public interface IHittable {
        void Hit(int damage, GameObject source);

        bool CanBeHit();

        GameObject GetGameObject();
    }
}