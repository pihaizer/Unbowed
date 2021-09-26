using UnityEngine;

namespace Gameplay {
    public interface IHittable {
        void Hit(float damage, GameObject source);

        bool CanBeHit();

        GameObject GetGameObject();
    }
}