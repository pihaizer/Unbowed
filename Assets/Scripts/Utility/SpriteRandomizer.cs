using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unbowed.Utility {
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRandomizer : MonoBehaviour {
        public List<SpriteWeight> sprites;

        private void Start() {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            var weightSum = sprites.Select(x => x.weight).Aggregate((sum, next) => sum + next);
            var randomValue = UnityEngine.Random.Range(0, weightSum - 1e-6f);
            var currentWeight = 0f;
            foreach (var spriteWeightPair in sprites) {
                if (currentWeight <= randomValue && randomValue < currentWeight + spriteWeightPair.weight) {
                    spriteRenderer.sprite = spriteWeightPair.sprite;
                    return;
                } else {
                    currentWeight += spriteWeightPair.weight;
                }
            }
        }

        [Serializable]
        public class SpriteWeight {
            public Sprite sprite;
            public float weight;
        }
    }
}
