using UnityEngine;

namespace Unbowed.Utility.Modifiers {
    public static class Operations {
        public static bool Or(bool b1, bool b2) => b1 || b2;

        public static bool And(bool b1, bool b2) => b1 && b2;
        

        public static int Add(int i1, int i2) => i1 + i2;

        public static float Add(float f1, float f2) => f1 + f2;

        public static Vector2 Add(Vector2 v1, Vector2 v2) => v1 + v2;

        public static Vector2Int Add(Vector2Int v1, Vector2Int v2) => v1 + v2;
        

        public static int Mul(int i1, int i2) => i1 * i2;

        public static float Mul(float f1, float f2) => f1 * f2;
    }
}