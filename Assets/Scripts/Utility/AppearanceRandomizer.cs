using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unbowed.Utility {
    [ExecuteInEditMode]
    public class AppearanceRandomizer : MonoBehaviour
    {
        public List<ObjectsWeight> objects;

        private void Start() {
            if (!Application.isPlaying) {
                objects[0].gameObject.SetActive(true);
                return;
            }
            var weightSum = objects.Select(x => x.weight).Aggregate((sum, next) => sum + next);
            var randomValue = UnityEngine.Random.Range(0, weightSum - 1e-6f);
            var currentWeight = 0f;
            foreach (var objectWeightPair in objects) {
                if (currentWeight <= randomValue && randomValue < currentWeight + objectWeightPair.weight) {
                    objectWeightPair.gameObject.SetActive(true);
                } else {
                    currentWeight += objectWeightPair.weight;
                }
            }
        }

        [Serializable]
        public class ObjectsWeight {
            public GameObject gameObject;
            public float weight;
        }
    }
}
