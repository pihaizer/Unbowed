using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;

namespace Unbowed.Utility {
    public static class Curves {
        public static float Evaluate(float x, Ease ease, Vector2 scope) {
            float normalizedY = EaseManager.Evaluate(ease, null, x, 1, 0, 0);
            return scope.x + normalizedY * (scope.y - scope.x);
        }

        public static int RandomIntOnEase(Ease ease, Vector2Int scope) {
            float value = Evaluate(Random.value, ease, scope + Vector2.up);
            return Mathf.FloorToInt(value);
        }
    }
}