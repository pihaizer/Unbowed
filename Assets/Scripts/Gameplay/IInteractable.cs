using UnityEngine;

namespace Unbowed.Gameplay {
    public interface IInteractable {
        void Interact(GameObject source);

        Transform GetTransform();
    }
}