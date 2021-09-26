using UnityEngine;

namespace Utility {
    public static class VectorRandom {
        public static float Range(Vector2 range) => Random.Range(range.x, range.y);
    }
}