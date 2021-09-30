using UnityEngine;
using UnityEngine.EventSystems;

namespace Unbowed.Utility {
    public static class VectorUtility {
        public static Vector2Int RotatedClockwise(this Vector2Int v) {
            return new Vector2Int(v.y, -v.x);
        }

        public static Vector2Int RotatedCounterClockwise(this Vector2Int v) {
            return new Vector2Int(-v.y, v.x);
        }

        public static Vector2 Rotate(this Vector2 v, float degrees) {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }

        public static Vector2Int Snap(this Vector2 v) => new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));

        public static MoveDirection ToMoveDirection(this Vector2Int v) {
            if (v == Vector2Int.zero) return MoveDirection.None;
            if (v.x == 0)
                if (v.y < 0)
                    return MoveDirection.Down;
                else
                    return MoveDirection.Up;
            if (v.y == 0) {
                if (v.x < 0) return MoveDirection.Left;
                return MoveDirection.Right;
            }

            return MoveDirection.None;
        }

        public static Vector2Int ToVector2Int(this MoveDirection m) {
            switch (m) {
                case MoveDirection.Down:
                    return Vector2Int.down;
                case MoveDirection.Left:
                    return Vector2Int.left;
                case MoveDirection.Right:
                    return Vector2Int.right;
                case MoveDirection.Up:
                    return Vector2Int.up;
            }

            return Vector2Int.zero;
        }

        public static Vector2 Abs(this Vector2 v) {
            return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }

        public static Vector2Int[] Directions(this Vector2Int v) {
            return new[] {
                v + Vector2Int.up,
                v + Vector2Int.down,
                v + Vector2Int.left,
                v + Vector2Int.right
            };
        }

        // public static readonly Vector2Int[] Directions = new Vector2Int[4] {
        //     Vector2Int.up,
        //     Vector2Int.down,
        //     Vector2Int.left,
        //     Vector2Int.right
        // };
    }
}