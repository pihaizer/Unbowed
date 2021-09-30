using UnityEngine;

namespace Unbowed.Utility {
    public static class GameObjectExtensions {
        public static void ToggleActive(this GameObject gameObject) =>
            gameObject.SetActive(!gameObject.activeSelf);
    }
}