using UnityEngine;

namespace Unbowed.Utility {
    public static class VectorRandom {
        public static float Range(Vector2 range) => Random.Range(range.x, range.y);
        public static int Range(Vector2Int range) => Random.Range(range.x, range.y + 1);
    }
}