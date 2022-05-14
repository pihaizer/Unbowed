using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unbowed.Utility {
    [Serializable]
    [HideLabel]
    public class Weights<T> {
        [SerializeField]
        [TableList(AlwaysExpanded = true, IsReadOnly = true, HideToolbar = true)]
        private List<Weight<T>> weights;

        public void SetValues(T[] values) {
            weights.SetLength(values.Length);
            for (int i = 0; i < weights.Count; i++) {
                var weight = weights[i];
                weight.value = values[i];
                weights[i] = weight;
            }
        }

        public T Random() => GetValue(UnityEngine.Random.value);

        public T GetValue(float randomValue) {
            if (weights.Count == 0) throw new ArgumentOutOfRangeException();
            float weightSum = weights[0].normalized;
            
            for (int i = 1; i < weights.Count; i++) {
                if (weightSum >= randomValue) return weights[i-1].value;
                weightSum += weights[i].normalized;
            }

            return weights[weights.Count - 1].value;
        }

        [Button]
        private void UpdateNormalizedValues() {
            float weightSum = weights.Sum(w => w.weight);
            for (int i = 0; i < weights.Count; i++) {
                var weight = weights[i];
                weight.normalized = weight.weight / weightSum;
                weights[i] = weight;
            }
        }

        [Serializable]
        private struct Weight<K> {
            [DisplayAsString, TableColumnWidth(15)]
            public K value;

            [Range(0, 1)]
            public float weight;

            [HideInInspector]
            public float normalized;

            [ShowInInspector, DisplayAsString, VerticalGroup("Normalized"), TableColumnWidth(40), HideLabel]
            private float NormalizedPercent => normalized * 100f;
        }
    }
}