using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay {
    public class ColliderZone : MonoBehaviour {
        public List<Collider> colliders;

        public bool Contains(Vector3 point) =>
            colliders.Any(collider => IsPointWithinCollider(collider, point));

        public Vector3 GetClosestPoint(Vector3 point) {
            float minDistance = float.MaxValue;
            var closest = new Vector3();
            
            foreach (var collider in colliders) {
                var currentClosest = collider.ClosestPoint(point);
                float currentDistance = Vector3.Distance(point, currentClosest);
                if (currentDistance < minDistance) {
                    closest = currentClosest;
                    minDistance = currentDistance;
                }
            }

            return closest;
        }

        bool IsPointWithinCollider(Collider collider, Vector3 point) => 
            (collider.ClosestPoint(point) - point).sqrMagnitude < Mathf.Epsilon * Mathf.Epsilon;
    }
}