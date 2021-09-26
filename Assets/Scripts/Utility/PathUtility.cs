using UnityEngine;
using UnityEngine.AI;

namespace Utility {
    public static class PathUtility {
        public static float GetLength(this NavMeshPath path) {
            float length = 0.0f;

            if (path.status != NavMeshPathStatus.PathInvalid && path.corners.Length > 1) {
                for (int i = 1; i < path.corners.Length; ++i) {
                    length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
            }

            return length;
        }
        
        public static float GetRemainingDistance(this NavMeshAgent agent) {
            return agent.path.GetLength();
        }
    }
}